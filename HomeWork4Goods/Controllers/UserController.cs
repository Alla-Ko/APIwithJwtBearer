using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork4Products.Controllers;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return BadRequest("Email and password are important");
        }
        var user = new IdentityUser
        {
            Email = email,
            UserName = email,
            EmailConfirmed = true

        };
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Products");
        }
        string errors = "";
        foreach (var item in result.Errors)
        {
            Console.WriteLine(item);
            errors = errors + " " + item.Description;
        }
        ViewBag.ErrorMessage = errors;
        return View();

    }
    [HttpGet]
    public IActionResult Auth()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Auth(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return BadRequest("Email and password are important");
        }

        var result = await _signInManager.PasswordSignInAsync(
            email,
            password,
            isPersistent: false,
            lockoutOnFailure: false
            );
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Products");
        }

        ViewBag.ErrorMessage = "Incorrect email or password combination";
        return View();

    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Auth", "User");
    }
}
