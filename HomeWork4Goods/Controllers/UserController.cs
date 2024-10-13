using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork4Products.Controllers;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
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
            ViewBag.ErrorMessage = "Email and password are important";
            return View();

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
            ViewBag.ErrorMessage = "Email and password are important";
            return View();

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
    [Authorize(Roles = "admin")]
    public IActionResult CreateRole()
    {
        return View();
    }
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
        {
            ViewBag.ErrorMessage = "RoleName is important";
            return View();

        }
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (roleExists)
        {
            ViewBag.ErrorMessage = $"The role {roleName} is already exists";
            return View();

        }
        var role = new IdentityRole { Name = roleName };
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Products");

        }

        ViewBag.ErrorMessage = result.ToString();
        return View();
    }
    [Authorize(Roles = "admin")]
    public IActionResult AssignRole()
    {
        return View();
    }
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
        {
            ViewBag.ErrorMessage = "userId or roleName are important";
            return View();

        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ViewBag.ErrorMessage = "The User is not found";
            return View();

        }
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            ViewBag.ErrorMessage = $"Role {roleName} not found";
            return View();

        }
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Products");
        }
        ViewBag.ErrorMessage = result.ToString();
        return View();
    }

}
