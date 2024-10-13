using HomeWork4Products.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork4Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public APIUserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
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
                return Ok(new { message = "User registered successfully." });
            }
            return BadRequest(result.Errors);
        }

        // POST: api/APIUser/Auth
        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody] AuthModel model)
        {
            string email = model.Email;
            string password = model.Password;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email and password are required.");
            }

            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent: false,
                lockoutOnFailure: false
                );
            if (result.Succeeded)
            {

                return Ok(new { message = "Authentication successful." });
            }

            return Unauthorized("Invalid login attempt.");

        }
        // POST: api/APIUser/Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully." });
        }

        // POST: api/APIUser/CreateRole
        [Authorize(Roles = "admin")]
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required.");
            }
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest($"Role {roleName} already exists.");
            }
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role {roleName} created successfully." });
            }

            return BadRequest(result.Errors);
        }

        // POST: api/APIUser/AssignRole
        [Authorize(Roles = "admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("UserId and roleName are required.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest($"Role {roleName} does not exist.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role {roleName} assigned to user {userId}." });
            }

            return BadRequest(result.Errors);
        }

    }
}
