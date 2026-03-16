using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Authentication.Commands.Register.NewYorker;

public class RegisterNewYorkerCommandHandler(
    UserManager<ApplicationUser> userManager,
    ITagRepository tagRepository,
    IEmailService emailService)
    : IRequestHandler<RegisterNewYorkerCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RegisterNewYorkerCommand request, CancellationToken cancellationToken)
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
                
                NewYorkerInfo = new NewYorkerInfo
                {
                    IsInterestedInVolunteering = request.IsInterestedInVolunteering,
                    IsOpenToAttendingLocalEvents = request.IsOpenToAttendingLocalEvents,
                    FollowNeighborhoodUpdates = request.FollowNeighborhoodUpdates,
                    MakeProfilePublic = request.MakeProfilePublic,
                    DisplayNeighborhood = request.DisplayNeighborhood,
                    AllowMessagesFromVerifiedOrganizations = request.AllowMessagesFromVerifiedOrganizations
                }
            }
        };
        
        if (request.Interests.Count != 0)
        {
            newUser.Profile.Interests = request.Interests
                .Select(category => new UserInterest
                {
                    Category = category
                })
                .ToList();
        }
        
        var tag = await tagRepository.GetByNameAsync("New Yorker", cancellationToken);
        newUser.Profile.Tags!.Add(new UserTag{ TagId = tag!.Id });
        
        // Use Identity's UserManager to create the user with the provided password
        var result = await userManager.CreateAsync(newUser, request.Password);
        
        // If the creation fails, return the errors
        if (!result.Succeeded)
        {
            var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
            var error = new ApiError("user.registration_failed", identityErrors);
            return StandardResponse.Failure(error);
        }
        
        await userManager.AddToRoleAsync(newUser, "Resident");
        
        // If everything is successful, return the standard success response
        var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var model = new WelcomeEmailModel(newUser.GetFullName(), request.Email, token);
        await emailService.SendWelcomeEmailAsync(request.Email, model, cancellationToken);
        
        return StandardResponse.Success();
    }
}