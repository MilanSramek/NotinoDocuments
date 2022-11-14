using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.ComponentModel.DataAnnotations;

namespace Documents.API;

public class DocumentModel
{
    [Required]
    public string Id { get; set; } = null!;
    public IEnumerable<string>? Tags { get; set; }

    [Required]
    public string Data { get; set; } = null!;
}
