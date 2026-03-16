using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Authentication.Commands.Register.NewYorker;

public record RegisterNewYorkerCommand (
    string FirstName, 
    string? LastName, 
    string Username,
    string Email, 
    string Password,
    AddressInputDto Address,
    List<Category> Interests,
    bool IsInterestedInVolunteering,
    bool IsOpenToAttendingLocalEvents,
    bool FollowNeighborhoodUpdates,
    bool MakeProfilePublic,
    bool DisplayNeighborhood,
    bool AllowMessagesFromVerifiedOrganizations
) : IRequest<StandardResponse>;