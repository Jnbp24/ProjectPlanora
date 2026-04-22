using System.ComponentModel.DataAnnotations;

namespace Planora.DTO.Auth;

public class PasswordChangeDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string OldPassword {get; set;}

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; }
}