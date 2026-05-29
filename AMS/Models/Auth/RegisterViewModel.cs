using System.ComponentModel.DataAnnotations;

namespace AMS.Models.Auth;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be at least 2 characters.")]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]

    [Display(Name = "Address")]
    public string Address { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    [Required]
    [Display(Name = "Account Type")]
    public string AccountType { get; set; }//Byer seller

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
}
