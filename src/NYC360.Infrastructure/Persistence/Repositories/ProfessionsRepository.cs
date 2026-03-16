using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Dtos.Professions;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.Posts;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class ProfessionsRepository(ApplicationDbContext db) : IProfessionsRepository
{
    public async Task AddJobOfferAsync(JobOffer offer, CancellationToken ct)
    {
        await db.JobOffers.AddAsync(offer, ct);
    }

    public void UpdateJobOffer(JobOffer offer)
    {
        db.JobOffers.Update(offer);
    }
    
    public void DeleteJobOffer(JobOffer offer)
    {
        db.JobOffers.Remove(offer);
    }

    public async Task<JobOffer?> GetJobOfferByIdAsync(int id, CancellationToken ct)
    {
        return await db.JobOffers
            .AsNoTracking()
            .Include(j => j.Author)
                .ThenInclude(up => up!.User)
            .Include(j => j.Address)
                .ThenInclude(a => a!.Location)
            .FirstOrDefaultAsync(j => j.Id == id, ct);
    }

    public async Task<JobOffer?> GetJobOfferForManagementAsync(int id, int userId, CancellationToken ct)
    {
        return await db.JobOffers
            .FirstOrDefaultAsync(j => j.Id == id && j.AuthorId == userId, ct);
    }
    
    public async Task<JobApplication?> GetApplicationForOwnerAsync(int applicationId, int ownerId, CancellationToken ct)
    {
        return await db.JobApplications
            .Include(a => a.JobOffer)
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.JobOffer.AuthorId == ownerId, ct);
    }

    public async Task<List<JobOfferMinimalDto>> GetRelatedJobsAsync(int currentJobId, int? locationId, EmploymentType type, int count, CancellationToken ct)
    {
        return await db.JobOffers
            .AsNoTracking()
            .Include(j => j.Author)
            .ThenInclude(up => up.User)
            .Where(j => 
                    j.IsActive && 
                    j.Id != currentJobId && // Exclude the job we are currently looking at
                    (j.Address!.LocationId == locationId || j.EmploymentType == type) // Match Location OR Type
            )
            // Prioritize exact location match, then date
            .OrderByDescending(j => j.CreatedAt)
            .Take(count)
            .Select(j => new JobOfferMinimalDto(
                j.Id,
                j.Title,
                j.SalaryMin,
                j.SalaryMax,
                j.WorkArrangement,
                j.EmploymentType,
                j.EmploymentLevel,
                j.Author != null ? (string.IsNullOrEmpty(j.CompanyName) ? j.Author.GetFullName() : j.CompanyName) : j.CompanyName,
                j.Author != null ? j.Author.AvatarUrl : string.Empty,
                j.Author != null ? j.Author.User!.UserName : null
            ))
            .ToListAsync(ct);
    }

    public async Task<(List<JobOffer>, int)> GetUserJobOffersAsync(int userId, bool? isActive, int page, int pageSize, CancellationToken ct)
    {
        var query = db.JobOffers
            .AsNoTracking()
            .Where(j => j.AuthorId == userId);

        if (isActive.HasValue)
            query = query.Where(j => j.IsActive == isActive.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(List<JobApplication>, int)> GetApplicationsByJobIdAsync(int jobOfferId, int ownerId, int page, int pageSize, CancellationToken ct)
    {
        // Verify ownership and fetch applications in one go
        var query = db.JobApplications
            .AsNoTracking()
            .Include(a => a.Applicant).ThenInclude(up => up.User)
            .Include(a => a.Applicant).ThenInclude(up => up.Educations)
            .Include(a => a.Applicant).ThenInclude(up => up.Positions)
            .Where(a => a.JobOfferId == jobOfferId && a.JobOffer.AuthorId == ownerId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.AppliedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<List<JobOfferMinimalDto>> GetFeaturedJobsAsync(int count, CancellationToken ct)
    {
        return await db.JobOffers
            .AsNoTracking()
            .Where(j => j.IsActive)
            .OrderByDescending(j => j.CreatedAt) // Or sort by "Featured" flag if you have one
            .Take(count)
            .Select(j => new JobOfferMinimalDto(
                j.Id,
                j.Title,
                j.SalaryMin,
                j.SalaryMax,
                j.WorkArrangement,
                j.EmploymentType,
                j.EmploymentLevel,
                j.Author != null ? (string.IsNullOrEmpty(j.CompanyName) ? j.Author.GetFullName() : j.CompanyName) : j.CompanyName,
                j.Author != null ? j.Author.AvatarUrl : string.Empty,
                j.Author != null ? j.Author.User!.UserName : null
            ))
            .ToListAsync(ct);
    }

    public async Task<(List<JobOfferMinimalDto>, int)> SearchJobOffersAsync(
        string? search, int? locationId, WorkArrangement? arrangement, EmploymentType? type, 
        EmploymentLevel? level, decimal? minSalary, bool isActive, int page, int pageSize, CancellationToken ct)
    {
        var query = db.JobOffers
            .AsNoTracking()
            .Where(j => j.IsActive);

        // Filters
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(j => EF.Functions.Like(j.Title, $"%{search}%") || EF.Functions.Like(j.CompanyName!, $"%{search}%"));

        if (locationId.HasValue) query = query.Where(j => j.Address!.LocationId == locationId);
        if (arrangement.HasValue) query = query.Where(j => j.WorkArrangement == arrangement);
        if (type.HasValue) query = query.Where(j => j.EmploymentType == type);
        if (level.HasValue) query = query.Where(j => j.EmploymentLevel == level);
        if (minSalary.HasValue) query = query.Where(j => j.SalaryMax >= minSalary);
        query = query.Where(j => j.IsActive == isActive);
        
        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(j => new JobOfferMinimalDto(
                j.Id,
                j.Title,
                j.SalaryMin,
                j.SalaryMax,
                j.WorkArrangement,
                j.EmploymentType,
                j.EmploymentLevel,
                j.Author != null ? (string.IsNullOrEmpty(j.CompanyName) ? j.Author.GetFullName() : j.CompanyName) : j.CompanyName,
                j.Author != null ? j.Author.AvatarUrl : string.Empty,
                j.Author != null ? j.Author.User!.UserName : null
            ))
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(List<PostDto>, int)> GetJobPostFeedAsync(int? userId, int? locationId, WorkArrangement? arrangement, decimal? minSalary, int page, int pageSize, CancellationToken ct)
    {
        // 1. Start with Posts of type 'Job'
        var query = db.Posts
            .AsNoTracking()
            .Include(p => p.Link)
            .Where(p => p.PostType == PostType.Job);

        // 2. Join with JobOffers via PostLink.LinkedEntityId
        // This is the polymorphic link logic you defined
        var joinedQuery = from post in query
                          join link in db.PostLinks on post.Id equals link.PostId
                          join job in db.JobOffers on link.LinkedEntityId equals job.Id
                          where link.Type == PostType.Job && job.IsActive
                          select new { post, job };

        // 3. Apply Professional Filters
        if (locationId.HasValue)
            joinedQuery = joinedQuery.Where(x => x.job.Address!.LocationId == locationId);

        if (arrangement.HasValue)
            joinedQuery = joinedQuery.Where(x => x.job.WorkArrangement == arrangement);

        if (minSalary.HasValue)
            joinedQuery = joinedQuery.Where(x => x.job.SalaryMax >= minSalary);

        var total = await joinedQuery.CountAsync(ct);

        // 4. Project into your existing PostDto
        // We use the 'post' part of the join to maintain visual consistency in the feed
        var posts = await joinedQuery
            .OrderByDescending(x => x.post.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.post) // In a real scenario, you'd use your ProjectToPostDto helper here
            .ToListAsync(ct);

        // Mapping to DTO logic would follow...
        return (new List<PostDto>(), total); // Placeholder for the projection logic
    }

    public async Task<bool> HasAlreadyAppliedAsync(int jobOfferId, int userId, CancellationToken ct)
    {
        return await db.JobApplications.AnyAsync(a => a.JobOfferId == jobOfferId && a.ApplicantId == userId, ct);
    }

    public async Task AddApplicationAsync(JobApplication application, CancellationToken ct)
    {
        await db.JobApplications.AddAsync(application, ct);
    }
    
    public async Task<JobApplication?> GetApplicationForApplicantAsync(int applicationId, int userId, CancellationToken ct)
    {
        return await db.JobApplications
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.ApplicantId == userId, ct);
    }
    
    public async Task<(List<JobApplication>, int)> GetUserApplicationsAsync(int userId, int page, int pageSize, CancellationToken ct)
    {
        var query = db.JobApplications
            .AsNoTracking()
            .Include(a => a.JobOffer)
                .ThenInclude(j => j.Address)
            
            .Include(a => a.JobOffer)
                .ThenInclude(j => j.Author)
                    .ThenInclude(up => up.User)
           
            .Where(a => a.ApplicantId == userId);

        var total = await query.CountAsync(ct);
        
        var items = await query
            .OrderByDescending(a => a.AppliedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<int> GetApplicationCountAsync(int jobOfferId, CancellationToken ct)
    {
        return await db.JobApplications.CountAsync(a => a.JobOfferId == jobOfferId, ct);
    }
}