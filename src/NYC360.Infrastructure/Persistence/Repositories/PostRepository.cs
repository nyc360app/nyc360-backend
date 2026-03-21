using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Dtos.Professions;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Enums;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class PostRepository(ApplicationDbContext db) : IPostRepository
{
    public async Task AddAsync(Post post, CancellationToken ct)
    {
        await db.Posts.AddAsync(post, ct);
    }

    public async Task AddAsync(List<Post> posts, CancellationToken ct)
    {
        await db.Posts.AddRangeAsync(posts, ct);
    }

    public async Task<Post?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await db.Posts
            .Include(p => p.Attachments)
            .Include(p => p.Stats)
            .Include(p => p.Tags)
            .Include(p => p.ParentPost)
            .Include(p => p.Topic)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }
    
    public async Task<Post?> GetByIdWithStatsAsync(int postId, CancellationToken ct)
    {
        return await db.Posts
            .Include(p => p.Stats)
            .FirstOrDefaultAsync(p => p.Id == postId, ct);
    }

    public async Task<PostDto?> GetPostByIdAsync(int id, int? userId, CancellationToken ct)
    {
        var baseQuery = db.Posts.Where(p => p.Id == id);
        return await ProjectToPostDto(baseQuery, userId).FirstOrDefaultAsync(ct);
    }

    public async Task<PostDetailsDto?> GetPostWithDetailsDtoByIdAsync(int id, int? userId, bool includeUnapproved, CancellationToken ct)
    {
        // 1. Fetch the main Post DTO using your existing projection logic
        var baseQuery = db.Posts.AsNoTracking().Where(p => p.Id == id);
        if (!includeUnapproved)
            baseQuery = baseQuery.Where(p => p.IsApproved);
        var postDto = await ProjectToPostDto(baseQuery, userId).FirstOrDefaultAsync(ct);
    
        if (postDto == null) return null;

        // 2. Fetch only TOP-LEVEL comments (ParentCommentId == null) 
        // to prevent duplicate data in the recursive tree
        var commentsList = await db.PostComments
            .AsNoTracking()
            .Include(pc => pc.User)!.ThenInclude(up => up!.User) // Required for UserMinimalInfoDto.Map
            .Include(pc => pc.Replies)
                .ThenInclude(r => r.User)!
                    .ThenInclude(up => up!.User)// Recursive Author Loading
            .Where(pc => pc.PostId == id && pc.ParentCommentId == null)
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync(ct);

        // 3. Map to DTO (The Map function you provided handles recursion)
        var commentDtos = commentsList.Select(PostCommentDto.Map).ToList();
        
        // 4. Fetch related posts (same Division/Category)
        var relatedPosts = await db.Posts
                .AsNoTracking()
                .Where(p => p.Id != id && p.IsApproved && p.ParentPostId == null) 
                .Where(p => p.Category == postDto.Category)
                .Include(p => p.Stats)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostMinimalDto(
                    p.Id,
                    p.Title ?? p.Content!.Substring(0, 100),
                    p.Attachments.OrderBy(a => a.Id).Select(a => a.Url).FirstOrDefault(),
                    p.Stats!.Comments
                ))
                .Take(4)
                .ToListAsync(ct);

        return new PostDetailsDto(postDto, commentDtos, relatedPosts);
    }

    public async Task<(List<PostDto>, int)> GetAllPaginatedAsync(int page, int pageSize, int? userId, Category? category, string? search, PostType? postType, PostSource? sourceType, int? authorId, bool includeUnapproved, CancellationToken ct)
    {
        var baseQuery = db.Posts.AsQueryable();

        if (!includeUnapproved)
            baseQuery = baseQuery.Where(p => p.IsApproved);

        // 1. Filtering Logic
        if (category is not null) baseQuery = baseQuery.Where(p => p.Category == category);
        if (postType is not null) baseQuery = baseQuery.Where(p => p.PostType == postType);
        if (sourceType is not null) baseQuery = baseQuery.Where(p => p.SourceType == sourceType);
        if (authorId is not null) baseQuery = baseQuery.Where(p => p.AuthorId == authorId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            baseQuery = baseQuery.Where(p =>
                EF.Functions.Like(p.Title!, $"%{search}%") ||
                EF.Functions.Like(p.Content!, $"%{search}%"));
        }

        var totalCount = await baseQuery.CountAsync(ct);

        // 2. Use the unified Projector to avoid constructor errors
        var list = await ProjectToPostDto(baseQuery.OrderByDescending(p => p.CreatedAt), userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);

        return (list, totalCount);
    }
    
    public async Task<(List<PostDto>, int)> GetAllByCommunityIdPaginatedAsync(int? userId, int communityId, int page, int pageSize, CancellationToken ct)
    {
        // 1. Define the base filtered query
        var baseQuery = db.Posts
            .AsNoTracking()
            .Where(p => p.CommunityId == communityId && p.IsApproved);
    
        // 2. Get the count from the filtered base query
        var totalCount = await baseQuery.CountAsync(ct);
    
        // 3. Apply projection, sorting, and pagination
        // We sort by CreatedAt DESC to show newest community posts first
        var list = await ProjectToPostDto(baseQuery.OrderByDescending(p => p.CreatedAt), userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    
        return (list, totalCount);
    }

    public async Task<(List<PostDto>, int)> GetTrendingPaginatedAsync(List<Category> userInterests, int page, int pageSize, int? userId, CancellationToken ct)
    {
        // Ensure we have interests; if not, return an empty list or handle as a general trending list
        if (userInterests == null || !userInterests.Any())
        {
            // Option 1: Return general trending or an empty set
            // return (new List<PostDto>(), 0); 
            
            // Option 2 (Recommended Fallback): Use all available categories for general trending
            userInterests = Enum.GetValues<Category>().ToList(); 
        }
        
        // --- 1. Base Query and Filtering ---
        var baseQuery = db.Posts
            .AsNoTracking()
            // Filter posts to only those whose Category matches a user's interest
            .Where(p => p.IsApproved)
            .Where(p => userInterests.Contains(p.Category));

        // --- 2. Calculate Total Count ---
        var totalCount = await baseQuery.CountAsync(ct);

        // --- 3. Complex Trending Query (with Scoring and User Interaction) ---
        var query = ProjectToPostDto(baseQuery.OrderByDescending(p => p.CreatedAt), userId);

        // Note on Projection: Mapping complex objects like Author and Attachments within a LINQ query can lead to multiple 
        // SQL queries per row. In production, consider moving this to a simpler projection and loading complex data separately.

        // --- 4. Pagination and Execution ---
        var list = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (list, totalCount);
    }

    public async Task<(List<PostDto>, int)> GetFeedByCommunityIdsAsync(int? userId, List<int> communityIds, int page, int pageSize, CancellationToken ct)
    {
        // 1. Filter posts that belong to the provided community IDs
        var baseQuery = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .Where(p => p.CommunityId != null && communityIds.Contains(p.CommunityId.Value));

        // 2. Count total results for pagination
        var total = await baseQuery.CountAsync(ct);

        // 3. Project to DTO, Sort, and Paginate
        var items = await ProjectToPostDto(baseQuery.OrderByDescending(p => p.CreatedAt), userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }
    
    public async Task<List<PostDto>> GetFeaturedPostsAsync(int? userId, int count, CancellationToken ct)
    {
        // 1. Start with the raw Entity query
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .OrderByDescending(p => p.CreatedAt); // 2. Sort BEFORE projecting

        // 3. Project to DTO last
        return await ProjectToPostDto(query.Take(count), userId).ToListAsync(ct);
    }

    public async Task<List<InterestGroupDto>> GetGroupedInterestPostsAsync(int? userId, List<Category> categories, int postsPerCategory, List<int> excludeIds, CancellationToken ct)
    {
        var result = new List<InterestGroupDto>();

        foreach (var category in categories)
        {
            // 1. Filter raw entities + Exclude already seen IDs
            var query = db.Posts
                .AsNoTracking()
                .Where(p => p.IsApproved)
                .Where(p => p.Category == category && !excludeIds.Contains(p.Id))
                .OrderByDescending(p => p.CreatedAt)
                .Take(postsPerCategory);

            // 2. Project to DTO
            var posts = await ProjectToPostDto(query, userId).ToListAsync(ct);

            if (posts.Any())
            {
                result.Add(new InterestGroupDto(category, posts));
            }
        }

        return result;
    }

    public async Task<List<PostDto>> GetDiscoveryPostsAsync(int? userId, List<Category> excludedCategories, List<int> excludeIds, int count, CancellationToken ct)
    {
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .Where(p => !excludedCategories.Contains(p.Category) && !excludeIds.Contains(p.Id))
            .OrderByDescending(p => p.CreatedAt) 
            .Take(count);

        return await ProjectToPostDto(query, userId).ToListAsync(ct);
    }

    public async Task<List<CommunityMinimalDto>> GetSuggestedCommunitiesAsync(int userId, List<Category> userInterests, int count, CancellationToken ct)
    {
        return await db.Communities
            .AsNoTracking()
            .Where(c => c.IsActive && !c.IsPrivate && c.Members.All(m => m.UserId != userId))
            .OrderByDescending(c => c.Members.Count)
            .Select(c => CommunityMinimalDto.Map(c))
            .Take(count)
            .ToListAsync(ct);
    }

    public async Task<List<PostDto>> GetRecentUserPostsAsync(int userId, int count, CancellationToken ct)
    {
        // 1. Prepare the base query (No Includes needed here)
        var query = db.Posts
            .AsNoTracking() // Recommended for read-only queries
            .Where(p => p.IsApproved)
            .Where(p => p.AuthorId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count);

        // 2. Pass the query into your projection function
        // userId is passed twice: once for filtering and once for the interaction joins
        var projectedQuery = ProjectToPostDto(query, userId);

        // 3. Execute the query
        return await projectedQuery.ToListAsync(ct);
    }

    public async Task<List<PostDto>> GetLatestPostsAsync(Category? division, int? userId, int limit, CancellationToken ct)
    {
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .Where(p => !division.HasValue || p.Category == division)
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit);

        return await ProjectToPostDto(query, userId).ToListAsync(ct);
    }
    public async Task<List<PostDto>> GetFeaturedPostsAsync(Category? division, int? userId, int limit, CancellationToken ct)
    {
        var oneMonthAgo = DateTime.UtcNow.AddDays(-30);

        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .Where(p => !division.HasValue || p.Category == division)
            .Where(p => p.CreatedAt >= oneMonthAgo)
            // FEATURED LOGIC: 
            // 1. Give a massive boost to posts explicitly marked 'IsFeatured' (Future update)
            // 2. Add weighted engagement (Likes/Comments/Shares)
            // 3. Subtract 'Age' factor (newer posts score higher)
            .OrderByDescending(p => 
                //(p.IsFeatured ? 1000 : 0) +  // Todo in the future, consider adding a "Featured" flag to the DB
                ((p.Stats != null ? p.Stats.Likes : 0) * 5) + 
                ((p.Stats != null ? p.Stats.Comments : 0) * 3) + 
                ((p.Stats != null ? p.Stats.Shares : 0) * 2))
            .ThenByDescending(p => p.CreatedAt)
            .Take(limit);

        return await ProjectToPostDto(query, userId).ToListAsync(ct);
    }

    public async Task<List<PostDto>> GetTrendingPostsAsync(Category? division, int? userId, int limit, CancellationToken ct)
    {
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .Where(p => !division.HasValue || p.Category == division)
            // Simple Trending Algorithm: (Likes * 2) + Comments + Shares
            // Filtered by posts from the last 7 days to keep it "Fresh"
            .Where(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-7))
            .OrderByDescending(p => 
                ((p.Stats != null ? p.Stats.Likes : 0) * 3) + 
                ((p.Stats != null ? p.Stats.Comments : 0) * 2) + 
                (p.Stats != null ? p.Stats.Shares : 0))
            .Take(limit);

        return await ProjectToPostDto(query, userId).ToListAsync(ct);
    }
    public async Task<List<string>> GetTrendingTagsAsync(int count, CancellationToken ct)
    {
        return await db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .SelectMany(p => p.Tags)
            .GroupBy(t => t.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Take(count)
            .ToListAsync(ct);
    }
    public async Task<List<string>> GetExistingLinksAsync(List<string> links, CancellationToken ct)
    {
        if (links == null || !links.Any()) return new List<string>();

        var dateLimit = DateTime.UtcNow.AddDays(-30);
        var foundLinks = new List<string>();

        // We check each link one-by-one because even within a scope, 
        // a single DbContext cannot handle multiple concurrent 'AnyAsync' calls.
        foreach (var link in links)
        {
            var exists = await db.Posts
                .AsNoTracking()
                .Where(p => p.CreatedAt >= dateLimit)
                .AnyAsync(p => p.Content.Contains(link), ct);

            if (exists) foundLinks.Add(link);
        }

        return foundLinks;
    }
    
    public async Task<(List<PostDto>, int)> GetPostsByTagPaginatedAsync(string tagName, int page, int pageSize, int? userId, CancellationToken ct)
    {
        // 1. Normalize the input tag name to match our DB storage
        var normalizedTag = tagName.Trim().ToLower().Replace(" ", "-");

        // 2. Start with a query that filters posts having this tag
        var baseQuery = db.Posts
            .Where(p => p.IsApproved)
            .Where(p => p.Tags.Any(t => t.Name == normalizedTag));

        var totalCount = await baseQuery.CountAsync(ct);

        // 3. Apply the global projection (handling user interactions/tags/stats)
        var query = ProjectToPostDto(baseQuery.OrderByDescending(p => p.CreatedAt), userId);

        // 4. Paginate and Execute
        var posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (posts, totalCount);
    }

    public async Task<List<PostDto>> SearchPostsAsync(string term, Category? div, int? userId, int limit, CancellationToken ct)
    {
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .Where(p => !div.HasValue || p.Category == div)
            .Where(p => p.Title!.Contains(term) || p.Content!.Contains(term) || p.Tags.Any(t => t.Name.Contains(term)))
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit);

        return await ProjectToPostDto(query, userId).ToListAsync(ct);
    }

    public async Task<(List<NewsSubmissionDto>, int)> GetPendingNewsSubmissionsAsync(int page, int pageSize, string? search, CancellationToken ct)
    {
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.Category == Category.News)
            .Where(p => p.SourceType == PostSource.User)
            .Where(p => p.ModerationStatus == PostModerationStatus.Pending);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(p =>
                EF.Functions.Like(p.Title!, $"%{search}%") ||
                EF.Functions.Like(p.Content!, $"%{search}%") ||
                (p.Author != null && EF.Functions.Like(p.Author.FirstName + " " + p.Author.LastName!, $"%{search}%")));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new NewsSubmissionDto(
                p.Id,
                p.Title,
                p.Content!,
                p.Author != null
                    ? new UserMinimalInfoDto(
                        p.Author.UserId,
                        p.Author.User!.UserName!,
                        p.Author.FirstName + " " + p.Author.LastName!,
                        p.Author.AvatarUrl,
                        p.Author.User.Type)
                    : null,
                p.Location != null
                    ? new LocationDto(
                        p.Location.Id,
                        p.Location.Borough,
                        p.Location.Code,
                        p.Location.NeighborhoodNet,
                        p.Location.Neighborhood,
                        p.Location.ZipCode)
                    : null,
                p.Topic != null
                    ? new TopicDto
                    {
                        Id = p.Topic.Id,
                        Name = p.Topic.Name,
                        Category = p.Topic.Category
                    }
                    : null,
                p.CreatedAt,
                p.LastUpdated,
                p.ModerationStatus,
                p.ModerationNote
            ))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<List<Tag>> EnsureTagsExistAsync(List<string> tagNames, CancellationToken ct)
    {
        if (tagNames == null || !tagNames.Any()) return new List<Tag>();

        // 1. Clean and normalize all incoming strings
        var normalizedNames = tagNames
            .Select(n => n.Trim().ToLower().Replace(" ", "-"))
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Distinct()
            .ToList();

        // 2. Fetch existing tags from DB
        var existingTags = await db.Tags
            .Where(t => normalizedNames.Contains(t.Name))
            .ToListAsync(ct);

        var existingNames = existingTags.Select(t => t.Name).ToList();
    
        // 3. Identify truly new tags
        var newTags = normalizedNames
            .Where(name => !existingNames.Contains(name))
            .Select(name => new Tag 
            { 
                Name = name,
            })
            .ToList();

        if (newTags.Any())
        {
            // Add new tags to the context (UnitOfWork will save them)
            await db.Tags.AddRangeAsync(newTags, ct);
        }

        // Return the combined list so the service can map them to posts
        return existingTags.Concat(newTags).ToList();
    }
    public async Task<(List<PostDto>, int)> GetSavedPostsPaginatedAsync(int userId, int page, int pageSize, Category? category, CancellationToken ct)
    {
        // 1. Get the base query from the SavedPosts join table
        var savedPostsQuery = db.UserSavedPosts
            .Where(s => s.UserId == userId);

        // Apply category filter if provided
        if (category.HasValue)
        {
            savedPostsQuery = savedPostsQuery.Where(s => s.Post!.Category == category.Value);
        }

        savedPostsQuery = savedPostsQuery.OrderByDescending(s => s.SavedAt);

        // 2. Get total count
        var totalCount = await savedPostsQuery.CountAsync(ct);

        // 3. Select the Post entity and apply the standard projector
        // This ensures saved posts look exactly like posts in the main feed
        var postsQuery = savedPostsQuery.Select(s => s.Post!);

        var list = await ProjectToPostDto(postsQuery, userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);

        return (list, totalCount);
    }
    
    public async Task<(List<PostDto>, int)> GetCultureStoriesPaginatedAsync(int? userId, string? tag, string? timeFrame, int? locationId, int page, int pageSize, CancellationToken ct)
    {
        // 1. Base Query: Culture Category only
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.Category == Category.Culture && p.IsApproved);

        // 2. Filter by Tag (e.g., "Museum", "Art")
        if (!string.IsNullOrWhiteSpace(tag) && !tag.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            // Normalize if needed, or assume exact match from UI
            query = query.Where(p => p.Tags.Any(t => t.Name == tag));
        }

        // 3. Filter by Location
        if (locationId.HasValue)
        {
            query = query.Where(p => p.LocationId == locationId.Value);
        }

        // 4. Filter by TimeFrame (Dropdown logic)
        if (!string.IsNullOrWhiteSpace(timeFrame))
        {
            var now = DateTime.UtcNow;
            query = timeFrame.ToLower() switch
            {
                "this_weekend" => query.Where(p => p.CreatedAt >= GetPreviousFriday(now)),
                "this_week"    => query.Where(p => p.CreatedAt >= now.AddDays(-7)),
                "this_month"   => query.Where(p => p.CreatedAt >= now.AddDays(-30)),
                _ => query
            };
        }

        // 5. Get Total Count
        var totalCount = await query.CountAsync(ct);

        // 6. Project, Sort, and Paginate
        // reusing your existing private ProjectToPostDto method
        var items = await ProjectToPostDto(query.OrderByDescending(p => p.CreatedAt), userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
    
    public async Task<(List<PostDto>, int)> GetUniversalPostsAsync(int userId, Category? category, int? locationId, string? search, PostType? type, int page, int pageSize, CancellationToken ct)
    {
        // 1. Start with the base query (Filtering stage)
        var query = db.Posts.AsNoTracking().Where(p => p.IsApproved);

        // 2. Apply Dynamic Filters
        if (category != null) 
            query = query.Where(p => p.Category == category.Value);
    
        if (locationId != null) 
            query = query.Where(p => p.LocationId == locationId.Value);
        
        if(type != null)
            query = query.Where(p => p.PostType == type.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => EF.Functions.Like(p.Title!, $"%{search}%") || 
                                     EF.Functions.Like(p.Content!, $"%{search}%"));
        
        // 3. Get Total Count before Pagination
        var total = await query.CountAsync(ct);

        // 4. Apply Pagination and Order
        var paginatedQuery = query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        // 5. Use your Projection function to transform into PostDto
        // This executes the complex joins (Likes, Saves, Jobs) only on the 20 results
        var items = await ProjectToPostDto(paginatedQuery, userId).ToListAsync(ct);

        return (items, total);
    }
    
    // --- PRIVATE HELPERS FOR DRY CODE ---
    private IQueryable<PostDto> GetTrendingQuery(List<Category> categories, int? userId)
    {
        var baseQuery = db.Posts
            .AsNoTracking()
            .Where(p => p.IsApproved)
            .Where(p => categories.Contains(p.Category));

        return ProjectToPostDto(baseQuery, userId)
            .OrderByDescending(p => p.CreatedAt); // You can swap this for your trendingScore logic
    }

    private IQueryable<PostDto> ProjectToPostDto(IQueryable<Post> query, int? userId)
    {
        return from post in query
               // 1. Join for User Interactions (Likes/Dislikes)
               join interaction in db.PostUserInteractions
                       .Where(i => userId.HasValue && i.UserId == userId.Value)
                   on post.Id equals interaction.PostId into interactions
               from userInteraction in interactions.DefaultIfEmpty()

               // 2. Join for Saved Posts
               join saved in db.UserSavedPosts
                       .Where(s => userId.HasValue && s.UserId == userId.Value)
                   on post.Id equals saved.PostId into savedPosts
               from isSaved in savedPosts.DefaultIfEmpty()

               // 3. Join for Polymorphic Links (Jobs, Events, etc.)
               join link in db.PostLinks on post.Id equals link.PostId into links
               from postLink in links.DefaultIfEmpty()

               join job in db.JobOffers on postLink.LinkedEntityId equals job.Id into jobs
               from jobOffer in jobs.DefaultIfEmpty()

               select new PostDto(
                   post.Id,
                   post.Title,
                   post.Content!,
                   post.SourceType,
                   post.PostType,
                   post.Category,

                   // --- MAIN LOCATION ---
                   post.Location != null ? new LocationDto(
                       post.Location.Id,
                       post.Location.Borough,
                       post.Location.Code,
                       post.Location.NeighborhoodNet,
                       post.Location.Neighborhood,
                       post.Location.ZipCode
                   ) : null,

                   // --- FULL PARENT POST PROJECTION ---
                   // This fixes the empty parent post issue by explicitly mapping its child collections
                   post.ParentPost != null ? new PostDto(
                       post.ParentPost.Id,
                       post.ParentPost.Title,
                       post.ParentPost.Content!,
                       post.ParentPost.SourceType,
                       post.ParentPost.PostType,
                       post.ParentPost.Category,
                       
                       // Parent Location
                       post.ParentPost.Location != null ? new LocationDto(
                           post.ParentPost.Location.Id,
                           post.ParentPost.Location.Borough,
                           post.ParentPost.Location.Code,
                           post.ParentPost.Location.NeighborhoodNet,
                           post.ParentPost.Location.Neighborhood,
                           post.ParentPost.Location.ZipCode
                       ) : null,
                       
                       null, // We stop recursion here to prevent infinite loops

                       // Parent Attachments
                       post.ParentPost.Attachments.Select(a => new AttachmentDto(a.Id, a.Url)).ToList(),
                       
                       // Parent Stats
                       post.ParentPost.Stats != null ? new PostStatsDto(
                           post.ParentPost.Stats.Views, 
                           post.ParentPost.Stats.Likes, 
                           post.ParentPost.Stats.Dislikes, 
                           post.ParentPost.Stats.Comments, 
                           post.ParentPost.Stats.Shares
                       ) : null,

                       post.ParentPost.CreatedAt,
                       post.ParentPost.LastUpdated,

                       // Parent Author Mapping
                       post.ParentPost.SourceId != null && post.ParentPost.Source != null 
                           ? new RssSourcePostDto(post.ParentPost.Source.Id, post.ParentPost.Source.Name!, post.ParentPost.Source.ImageUrl)
                           : post.ParentPost.AuthorId != null && post.ParentPost.Author != null 
                               ? new UserMinimalInfoDto(
                                   post.ParentPost.Author.UserId, 
                                   post.ParentPost.Author.User!.UserName!, 
                                   post.ParentPost.Author.FirstName + " " + post.ParentPost.Author.LastName!, 
                                   post.ParentPost.Author.AvatarUrl, 
                                   post.ParentPost.Author.User!.Type
                                 )
                               : null,

                       // Parent Tags
                       post.ParentPost.Tags.Select(t => t.Name).ToList(),
                       
                       false, // isSaved (usually not needed for nested parent)
                        null,  // interaction (usually not needed for nested parent)
                        null,  // nested linked resource
                        null   // nested topic
                    ) : null,

                   // --- MAIN ATTACHMENTS ---
                   post.Attachments.Select(a => new AttachmentDto(a.Id, a.Url)).ToList(),

                   // --- MAIN STATS ---
                   post.Stats != null ? new PostStatsDto(
                       post.Stats.Views, 
                       post.Stats.Likes, 
                       post.Stats.Dislikes, 
                       post.Stats.Comments, 
                       post.Stats.Shares
                   ) : null,

                   post.CreatedAt,
                   post.LastUpdated,

                   // --- MAIN AUTHOR ---
                   post.SourceId != null && post.Source != null 
                       ? new RssSourcePostDto(post.Source.Id, post.Source.Name!, post.Source.ImageUrl)
                       : post.AuthorId != null && post.Author != null 
                           ? new UserMinimalInfoDto(
                               post.Author.UserId, 
                               post.Author.User!.UserName, 
                               post.Author.FirstName + " " + post.Author.LastName!, 
                               post.Author.AvatarUrl, 
                               post.Author.User.Type
                             )
                           : null,

                   // --- MAIN TAGS ---
                   post.Tags.Select(t => t.Name).ToList(),

                   isSaved != null,
                   userInteraction != null ? userInteraction.Type : null,

                   // --- LINKED RESOURCE (JOB/EVENT) ---
                   post.PostType == PostType.Job && jobOffer != null 
                       ? new JobOfferMinimalDto(
                           jobOffer.Id,
                           jobOffer.Title,
                           jobOffer.SalaryMin,
                           jobOffer.SalaryMax,
                           jobOffer.WorkArrangement,
                           jobOffer.EmploymentType,
                           jobOffer.EmploymentLevel,
                           jobOffer.Author != null ? (string.IsNullOrEmpty(jobOffer.CompanyName) ? jobOffer.Author.GetFullName() : jobOffer.CompanyName) : jobOffer.CompanyName,
                           jobOffer.Author!.AvatarUrl,
                            jobOffer.Author != null ? jobOffer.Author.User!.UserName : null
                        ) : null,
                   
                   post.Topic != null ? new TopicDto
                   {
                       Id = post.Topic.Id,
                       Name = post.Topic.Name,
                       Category = post.Topic.Category
                   } : null
               );
    }
    
    public async Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        return await db.Posts.AnyAsync(p => p.Id == id, ct);
    }

    public void Update(Post post)
    {
        db.Posts.Update(post); 
    }

    public void Remove(Post post)
    {
        db.Posts.Remove(post);
    }

    public async Task<(List<PostDto>, int)> GetPostsByTopicPaginatedAsync(int topicId, int page, int pageSize, int? userId, CancellationToken ct)
    {
        var baseQuery = db.Posts.Where(p => p.TopicId == topicId && p.IsApproved);
        var total = await baseQuery.CountAsync(ct);
        var items = await ProjectToPostDto(baseQuery.OrderByDescending(p => p.CreatedAt), userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        return (items, total);
    }
    
    private static DateTime GetPreviousFriday(DateTime date)
    {
        while (date.DayOfWeek != DayOfWeek.Friday) date = date.AddDays(-1);
        return date.Date;
    }
    
    // housing
    // In PostRepository.cs
    public async Task<List<PostDto>> GetRecentHousingPostsAsync(int? userId, int limit, CancellationToken ct)
    {
        var query = db.Posts
            .AsNoTracking()
            .Where(p => p.PostType == PostType.Housing && p.IsApproved)
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit);

        return await ProjectToPostDto(query, userId).ToListAsync(ct);
    }

    public async Task<PostAnalysisDto> GetMyPostsAnalysisByCategoryAsync(int userId, Category category, CancellationToken ct)
    {
        var baseQuery = db.Posts
            .Where(p => p.AuthorId == userId && p.Category == category);

        var totals = await baseQuery
            .GroupBy(p => 1)
            .Select(g => new
            {
                Count = g.Count(),
                Views = g.Sum(p => (long)p.Stats!.Views),
                Likes = g.Sum(p => (long)p.Stats!.Likes),
                Dislikes = g.Sum(p => (long)p.Stats!.Dislikes),
                Comments = g.Sum(p => (long)p.Stats!.Comments),
                Shares = g.Sum(p => (long)p.Stats!.Shares)
            })
            .FirstOrDefaultAsync(ct);

        if (totals == null || totals.Count == 0)
        {
            return new PostAnalysisDto(0, 0, 0, 0, 0, 0, 0, null);
        }

        // Top post by engagement (Likes + Comments + Shares)
        var topPostEntity = await baseQuery
            .OrderByDescending(p => p.Stats!.Likes + p.Stats!.Comments + p.Stats!.Shares)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Content,
                ImageUrl = p.Attachments.OrderBy(a => a.Id).Select(a => a.Url).FirstOrDefault(),
                p.Stats!.Comments
            })
            .FirstOrDefaultAsync(ct);

        PostMinimalDto? topPost = topPostEntity != null
            ? new PostMinimalDto(
                topPostEntity.Id,
                topPostEntity.Title ?? (topPostEntity.Content != null ? (topPostEntity.Content.Length > 50 ? topPostEntity.Content.Substring(0, 50) + "..." : topPostEntity.Content) : "Untitled"),
                topPostEntity.ImageUrl,
                topPostEntity.Comments)
            : null;

        double engagementRate = totals.Views > 0 
            ? (double)(totals.Likes + totals.Comments + totals.Shares) / totals.Views * 100 
            : 0;

        return new PostAnalysisDto(
            totals.Count,
            totals.Views,
            totals.Likes,
            totals.Dislikes,
            totals.Comments,
            totals.Shares,
            Math.Round(engagementRate, 2),
            topPost
        );
    }
}
