using NYC360.Domain.Enums;

namespace NYC360.Application.Features.SpaceListings.Common;

public static class SpaceListingDepartmentPolicy
{
    public static List<Category> BuildCategories(Category department, List<Category>? categories)
    {
        var result = categories?.Distinct().ToList() ?? [];
        if (!result.Contains(department))
            result.Insert(0, department);

        return result;
    }
}
