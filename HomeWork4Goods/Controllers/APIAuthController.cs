using HomeWork4Products.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeWork4Products.Controllers;

[Route("api/[controller]")]
[ApiController]
public class APIAuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    public APIAuthController
        (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IConfiguration configuration

        )
    {
        _userManager = userManager;
        _configuration = configuration;
        _signInManager = signInManager;

    }
    //POST
    //https:localhost:[port]/api/apiauth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        IdentityUser user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            {
                return Ok(new { message = "User registered successfully" });
            }
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }
    //POST
    //https:localhost:[port]/api/apiauth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Invalid login attempt");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (result.Succeeded)
        {
            var token = await GenerateJwtToken(user); // Передаємо тільки користувача
            return Ok(new { Token = token });
        }
        else
        {
            return Unauthorized("Invalid login attempt");
        }
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        // Отримуємо ролі користувача
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // Ім'я користувача
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Унікальний ідентифікатор токена
        };

        // Додаємо ролі як заяви
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); // Додаємо роль як claim
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token); // Повертаємо JWT як рядок
    }

}
