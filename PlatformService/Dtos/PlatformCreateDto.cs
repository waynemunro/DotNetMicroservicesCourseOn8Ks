
using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos;
public class PlatformCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Publisher { get; set; } = string.Empty;

    public string Developer { get; set; } = string.Empty;

    public string Cost { get; set; } = string.Empty;
}
