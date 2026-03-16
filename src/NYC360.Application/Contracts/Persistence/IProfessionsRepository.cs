using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Professions;

namespace NYC360.Application.Contracts.Persistence;

public interface IProfessionsRepository
{
    // Command Side
    Task AddJobOfferAsync(JobOffer offer, CancellationToken ct);
    void UpdateJobOffer(JobOffer offer);
    void DeleteJobOffer(JobOffer offer);
    Task<JobOffer?> GetJobOfferByIdAsync(int id, CancellationToken ct);
    Task<JobApplication?> GetApplicationForOwnerAsync(int applicationId, int ownerId, CancellationToken ct);
    
    // Query Side - Direct Table Access
    Task<JobOffer?> GetJobOfferForManagementAsync(int id, int userId, CancellationToken ct);
    Task<List<JobOfferMinimalDto>> GetRelatedJobsAsync(int currentJobId, int? locationId, EmploymentType type, int count, CancellationToken ct);
    Task<(List<JobOffer>, int)> GetUserJobOffersAsync(int userId, bool? isActive, int page, int pageSize, CancellationToken ct);
    Task<(List<JobApplication>, int)> GetApplicationsByJobIdAsync(int jobOfferId, int ownerId, int page, int pageSize, CancellationToken ct);
    Task<List<JobOfferMinimalDto>> GetFeaturedJobsAsync(int count, CancellationToken ct);
    Task<(List<JobOfferMinimalDto>, int)> SearchJobOffersAsync(string? search, int? locationId, WorkArrangement? arrangement, EmploymentType? type, EmploymentLevel? level, decimal? minSalary, bool isActive, int page, int pageSize, CancellationToken ct);
    
    // Query Side - The "Social" Feed (Joined)
    Task<(List<PostDto>, int)> GetJobPostFeedAsync(int? userId, int? locationId, WorkArrangement? arrangement, decimal? minSalary, int page, int pageSize, CancellationToken ct);
    
    // Applications
    Task<bool> HasAlreadyAppliedAsync(int jobOfferId, int userId, CancellationToken ct);
    Task AddApplicationAsync(JobApplication application, CancellationToken ct);
    Task<JobApplication?> GetApplicationForApplicantAsync(int applicationId, int userId, CancellationToken ct);
    Task<(List<JobApplication>, int)> GetUserApplicationsAsync(int userId, int page, int pageSize, CancellationToken ct);
    Task<int> GetApplicationCountAsync(int jobOfferId, CancellationToken ct);
}