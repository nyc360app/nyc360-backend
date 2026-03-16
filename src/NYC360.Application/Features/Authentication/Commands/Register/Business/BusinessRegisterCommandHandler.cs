using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Domain.Enums;

namespace NYC360.Application.Features.Authentication.Commands.Register.Business;

public class BusinessRegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    ILocationRepository locationRepository,
    ITagRepository tagRepository,
    IEmailService emailService)
    : IRequestHandler<BusinessRegisterCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(BusinessRegisterCommand request, CancellationToken ct)
    {
        // Check if the user already exists by email
        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        if (userByEmail is not null)
        {
            var error = new ApiError("user.duplicate_email", "Email is already in use.");
            return StandardResponse.Failure(error);
        }
        
        var addressId = await locationRepository.GetOrCreateAddressIdAsync(request.Address, ct);
        
        // Create the new user entity
        var newUser = new ApplicationUser
        {
            Type = UserType.Normal,
            Email = request.Email,
            UserName = request.Username,
            
            Profile = new UserProfile
            {
                FirstName = request.Name,
                Bio = request.Description,
                AddressId = addressId,
                
                BusinessInfo = new BusinessInfo
                {
                    Industry = request.Industry,
                    BusinessSize = request.BusinessSize,
                    ServiceArea = request.ServiceArea,
                    Services = request.Services,
                    OwnershipType = request.OwnershipType,
                    IsPublic = request.MakeProfilePublic,
                    IsLicensedInNyc = request.IsLicensedInNyc,
                    IsInsured = request.IsInsured
                }
            },
        };

        if (request.SocialLinks.Count != 0)
        {
            foreach (var link in request.SocialLinks)
            {
                newUser.Profile.SocialLinks.Add(new UserSocialLink
                {
                    Platform = link.Platform,
                    Url = link.Url
                });
            }
        }

        if (!string.IsNullOrEmpty(request.Website))
        {
            newUser.Profile.SocialLinks.Add(new UserSocialLink
            {
                Platform = SocialPlatform.Website,
                Url = request.Website
            });
        }
        
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            newUser.Profile.SocialLinks.Add(new UserSocialLink
            {
                Platform = SocialPlatform.PhoneNumber,
                Url = request.PhoneNumber
            });
        }
        
        var tag = await tagRepository.GetByNameAsync("NYC Business", ct);
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
        
        await userManager.AddToRoleAsync(newUser, "Business");
        var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var model = new WelcomeEmailModel(newUser.GetFullName(), request.Email, token);
        await emailService.SendWelcomeEmailAsync(request.Email, model, ct);
        
        return StandardResponse.Success();
    }
}