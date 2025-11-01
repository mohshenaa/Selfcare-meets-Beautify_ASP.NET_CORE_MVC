using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Selfcare_meets_Beautify.Model;
using Selfcare_meets_Beautify.ViewModels;

namespace Selfcare_meets_Beautify.Controllers
{

  public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AccountController> logger) : Controller
  {



    [AllowAnonymous]
    public IActionResult Login(string ReturnUrl = "/")
    {
      return View(new LoginModel() { ReturnUrl = ReturnUrl });
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
      if (ModelState.IsValid)
      {
        var user = userManager.Users.FirstOrDefault(u => u.UserName == model.UserName || u.Email == model.UserName);

        if (user == null)
        {
          ModelState.AddModelError("UserName", "Invalid user");
          return View(model);
        }

        var signResult = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

        if (signResult.Succeeded)
        {
          logger.LogInformation($"{model.UserName} logged in at {DateTime.Now}");



          return RedirectPermanent(model.ReturnUrl);

        }

        else
        {
          ModelState.AddModelError("Password", "Invalid credentials");
          return View(model);
        }

      }



      return View();
    }



    [AllowAnonymous]
    public IActionResult Register(string ReturnUrl = "/")
    {
      return View(new RegisterModel() { ReturnUrl = ReturnUrl });
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
      if (ModelState.IsValid)
      {
        var newUser = new AppUser()
        {
          UserName = model.UserName,
          Email = model.UserName
        }
        ;


        var result = await userManager.CreateAsync(newUser, model.Password);


        if (result.Succeeded)
        {
          await signInManager.CheckPasswordSignInAsync(newUser, model.Password, false);
          logger.LogInformation($"{model.UserName} registered in at {DateTime.Now}");
          return RedirectPermanent(model.ReturnUrl);
        }
        else
        {
          foreach (var err in result.Errors)
          {
            ModelState.AddModelError(err.Code, err.Description);
          }
          return View(model);
        }

      }




      return View(model);
    }



    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
      await signInManager.SignOutAsync();
      logger.LogInformation($"{HttpContext.User.Identity.Name} logged out at {DateTime.Now}");

      return RedirectPermanent("/");
    }
  }
}
