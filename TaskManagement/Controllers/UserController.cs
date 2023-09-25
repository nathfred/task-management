using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Models;
using TaskManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Implement user registration logic
            try
            {
                var user = new User
                {
                    FullName = model.Email, // for easy development purpose only
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return Ok("User registered successfully");

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return BadRequest("Invalid login attempt.");

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                    return Ok("User logged in successfully");

                return BadRequest("Invalid login attempt.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!int.TryParse(id, out int parsedId))
            {
                return BadRequest("Invalid user ID. Please provide a valid integer.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == int.Parse((id)));
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] string email, [FromForm] string password)
        {
            // Validate input if needed
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return BadRequest(new { error = "Email and password are required." });

            try
            {
                var user = new User
                {
                    FullName = email, // for easy development purpose only
                    UserName = email,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    return Ok(new { message = "User created successfully" });

                // Log the errors for debugging
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code}, Description: {error.Description}");
                }

                return BadRequest(new { errors = result.Errors });
            }
            catch (Exception ex)
            {
                // Log the inner exception for debugging
                Console.WriteLine($"Inner Exception: {ex.InnerException}");

                return StatusCode(500, new { error = $"An error occurred: {ex.InnerException}" });
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}" });
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromForm] string newEmail)
        {
            // Validate input if needed
            if (string.IsNullOrWhiteSpace(newEmail))
                return BadRequest(new { error = "New email is required." });

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == int.Parse((id)));
                if (user == null)
                    return NotFound(new { error = "User not found." });

                user.Email = newEmail;
                user.UserName = newEmail;
                user.FullName = newEmail;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return Ok(new { message = "User profile updated successfully" });

                return BadRequest(new { errors = result.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}" });
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == int.Parse((id)));
            if (user == null)
                return NotFound(new { error = "User not found." });

            try
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return Ok(new { message = "User deleted successfully" });

                return BadRequest(new { errors = result.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}" });
            }
        }

    }
}
