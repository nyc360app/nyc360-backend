using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Authentication.Commands.Register.Organization;

public class RegisterOrganizationCommandHandler(
    UserManager<ApplicationUser> userManager,
    ITagRepository tagRepository,
    ILocationRepository locationRepository,
    IEmailService emailService)
    : IRequestHandler<RegisterOrganizationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RegisterOrganizationCommand request, CancellationToken ct)
    {
        // Check if the user already exists by email
        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        if (userByEmail is not null)
        {
            var error = new ApiError("user.duplicate_email", "Email is already in use.");
            return StandardResponse.Failure(error);
        }
        
        // Create the new user entity
        var addressId = await locationRepository.GetOrCreateAddressIdAsync(request.Address, ct);
        
        var newUser = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Username,
            Type = UserType.Organization,
            
            Profile = new UserProfile
            {
                FirstName = request.Name,
                Bio = request.Description,
                AddressId = addressId,
                
                OrganizationInfo = new OrganizationInfo
                {
                    OrganizationType = request.OrganizationType,
                    ServiceArea = request.ServiceArea,
                    FundType = request.FundType,
                    IsTaxExempt = request.IsTaxExempt,
                    IsNysRegistered = request.IsNysRegistered,
                    Services = request.Services.Select(service => new OrganizationService { Service = service }).ToList()
                }
            }
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
        
        if (!string.IsNullOrEmpty(request.PublicEmail))
        {
            newUser.Profile.SocialLinks.Add(new UserSocialLink
            {
                Platform = SocialPlatform.Email,
                Url = request.PublicEmail
            });
        }
        
        if (request.Interests.Count != 0)
        {
            newUser.Profile!.Interests = request.Interests
                .Select(category => new UserInterest
                {
                    Category = category
                })
                .ToList();
        }
        
        var tag = await tagRepository.GetByNameAsync("NYC Organization", ct);
        newUser.Profile.Tags!.Add(new UserTag { TagId = tag!.Id });
        
        // Use Identity's UserManager to create the user with the provided password
        var result = await userManager.CreateAsync(newUser, request.Password);
        
        // If the creation fails, return the errors
        if (!result.Succeeded)
        {
            var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
            var error = new ApiError("user.registration_failed", identityErrors);
            return StandardResponse.Failure(error);
        }
        
        await userManager.AddToRoleAsync(newUser, "Organization");
        
        // If everything is successful, return the standard success response
        var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var model = new WelcomeEmailModel(newUser.GetFullName(), request.Email, token);
        await emailService.SendWelcomeEmailAsync(request.Email, model, ct);
        
        return StandardResponse.Success();
    }
}