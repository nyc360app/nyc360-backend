using NYC360.Infrastructure.Persistence.Repositories.Authentication;
using NYC360.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NYC360.Application.Contracts.Persistence;

namespace NYC360.Infrastructure.Extensions;

public static class RepositoryRegistration
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserInterestRepository, UserInterestRepository>();
        services.AddScoped<IUserSavedPostRepository, UserSavedPostRepository>();
        
        services.AddScoped<IRssSourceRepository, RssSourceRepository>();
        services.AddScoped<IRssFeedConnectionRequestRepository, RssFeedConnectionRequestRepository>();
        services.AddScoped<IRssFeedItemRepository, RssFeedItemRepository>();
        
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IPostCommentRepository, PostCommentRepository>();
        services.AddScoped<IPostInteractionRepository, PostInteractionRepository>();
        
        services.AddScoped<IPostFlagRepository, PostFlagRepository>();
        
        services.AddScoped<IEventRepository, EventRepository>();
        
        services.AddScoped<ILocationRepository, LocationRepository>();

        services.AddScoped<ICommunityRepository, CommunityRepository>();
        
        services.AddScoped<IProfessionsRepository, ProfessionsRepository>();
        
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IVerificationRepository, VerificationRepository>();

        services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
        
        services.AddScoped<IHousingRequestRepository, HousingRequestRepository>();
        services.AddScoped<IHouseInfoRepository, HouseInfoRepository>();
        services.AddScoped<IHouseListingAuthorizationRepository, HouseListingAuthorizationRepository>();

        services.AddScoped<ISpaceListingRepository, SpaceListingRepository>();
        
        services.AddScoped<IForumRepository, ForumRepository>();
        services.AddScoped<IForumQuestionRepository, ForumQuestionRepository>();
        services.AddScoped<IForumAnswerRepository, ForumAnswerRepository>();
        
    }
}
