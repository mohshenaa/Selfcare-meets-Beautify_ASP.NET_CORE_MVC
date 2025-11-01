using Microsoft.AspNetCore.Identity;

namespace Selfcare_meets_Beautify.Model
{
  public class AppUser: IdentityUser<Guid>
  {
    public string? ProfilePicture { get; set; }
  }
  public class AppRole : IdentityRole<Guid>
  {
    public string? Description { get; set; }
  }
}
