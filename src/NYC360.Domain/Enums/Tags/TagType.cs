namespace NYC360.Domain.Enums.Tags;

public enum TagType 
{
    Identity = 1,     // Hard-coded roles (New Yorker, Org)
    Professional = 2, // From your Culture/Professions lists
    Interest = 3,     // Topic-based for feed filtering
    Location = 4      // Neighborhood/Borough tags
}