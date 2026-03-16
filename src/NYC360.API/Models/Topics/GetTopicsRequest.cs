using NYC360.Domain.Enums;

namespace NYC360.API.Models.Topics;

public class GetTopicsRequest
{
    public Category? Category { get; set; }
}
