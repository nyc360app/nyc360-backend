// using NYC360.Infrastructure.Persistence.DbContexts;
// using Microsoft.EntityFrameworkCore;
// using NYC360.Domain.Entities.User;
// using NYC360.Domain.Enums;
//
// namespace NYC360.Infrastructure.Persistence.CompiledQueries;
//
// public static class UserCompiledQueries
// {
//     public static readonly Func<ApplicationDbContext, string, Task<ApplicationUser?>> GetUserWithProfile =
//         EF.CompileAsyncQuery((ApplicationDbContext db, string normalized) =>
//             db.Users
//                 .Where(u => u.NormalizedUserName == normalized)
//                 .Select(u => new ApplicationUser
//                 {
//                     Id = u.Id,
//                     UserName = u.UserName,
//                     Type = u.Type,
//                     RegularProfile = u.Type == UserType.Normal
//                         ? u.RegularProfile
//                         : null,
//                     OrganizationProfile = u.Type == UserType.Organization
//                         ? u.OrganizationProfile
//                         : null,
//                     AdminProfile = u.Type == UserType.Admin
//                         ? u.AdminProfile
//                         : null
//                 })
//                 .FirstOrDefault()
//         );
// }