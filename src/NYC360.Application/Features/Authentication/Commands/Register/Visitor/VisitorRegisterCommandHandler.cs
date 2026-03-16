using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Authentication.Commands.Register.Visitor;

public class VisitorRegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    ITagRepository tagRepository,
    IEmailService emailService)
    : IRequestHandler<VisitorRegisterCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(VisitorRegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if the user already exists by email
        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        if (userByEmail is not null)
        {
            var error = new ApiError("user.duplicate_email", "Email is already in use.");
            return StandardResponse.Failure(error);
        }
        
        // Create the new user entity
        var newUser = new ApplicationUser
        {
            Type = UserType.Normal,
            Email = request.Email,
            UserName = request.Username,
            Profile = new UserProfile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                
                VisitorInfo = new VisitorInfo
                {
                    CityOfOrigin = request.CityOfOrigin,
                    CountryOfOrigin = request.CountryOfOrigin,
                    VisitPurpose = request.VisitPurpose,
                    LengthOfStay = request.LengthOfStay,
                    ReceiveEventAndCultureRecommendations = request.ReceiveEventAndCultureRecommendations,
                    EnableLocationBasedSuggestions = request.EnableLocationBasedSuggestions,
                    SavePlacesEventsGuides = request.SavePlacesEventsGuides,
                    DiscoverableProfile = request.DiscoverableProfile,
                    AllowMessagesFromNycPartners = request.AllowMessagesFromNycPartners
                }
            },
        };
        
        var tag = await tagRepository.GetByNameAsync("NYC Visitors", cancellationToken);
        newUser.Profile.Tags!.Add(new UserTag{ TagId = tag!.Id });
        
        if (request.Interests.Count != 0)
        {
            newUser.Profile.Interests = request.Interests
                .Select(category => new UserInterest
                {
                    Category = category
                })
                .ToList();
        }
        
        var result = await userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded)
        {
            var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
            var error = new ApiError("user.registration_failed", identityErrors);
            return StandardResponse.Failure(error);
        }
        
        await userManager.AddToRoleAsync(newUser, "Visitor");
        var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var model = new WelcomeEmailModel(newUser.GetFullName(), request.Email, token);
        await emailService.SendWelcomeEmailAsync(request.Email, model, cancellationToken);
        
        return StandardResponse.Success();
    }
}