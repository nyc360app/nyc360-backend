using System.ComponentModel.DataAnnotations;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Entities;

public class VerificationDocument
{
    [Key]
    public int Id { get; set; }
    public string FileUrl { get; set; }
    public DocumentType DocumentType { get; set; }
}