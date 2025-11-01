using System.ComponentModel.DataAnnotations;

namespace Selfcare_meets_Beautify.ViewModels
{
  public class LoginModel
  {
    [EmailAddress]
    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; } = default!;
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 4)]
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } = "/";


  }
}
