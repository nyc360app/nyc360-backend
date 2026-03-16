using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Dtos;

public record RoleDto(int Id, string Name, List<string>? Permissions, int ContentLimit);

public static class RoleDtoExtension
{
    extension(RoleDto)
    {
        public static RoleDto Map(ApplicationRole role, List<string>? permissions)
        {
            return new RoleDto(role.Id, role.Name!, permissions, role.ContentLimit);
        }
    }
}