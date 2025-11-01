using System.ComponentModel.DataAnnotations;

namespace Selfcare_meets_Beautify.ViewModels
{
  public class RegisterModel
  {
    [EmailAddress]
    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; } = default!;
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 4)]
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = default!;
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = default!;

    public string ReturnUrl { get; set; } = "/";
  }
}
