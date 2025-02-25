using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        _logger.LogInformation("Registering new user: {Email}", model.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Registration failed for email {Email} due to invalid model state.", model.Email);   
            return BadRequest(ModelState);
        }

        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Registration failed for email {Email} due to errors: {Errors}", model.Email, errors);
            return BadRequest(result.Errors);

        }

        await _userManager.AddToRoleAsync(user, "User");
        _logger.LogInformation("User {Email} registered successfully.", model.Email);

        return Ok("User created successfully!");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        _logger.LogInformation("Login attempt for email: {Email}", model.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Login failed for {Email} due to invalid model state.", model.Email);
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User with email {Email} not found.", model.Email);
            return Unauthorized("Invalid email or password.");
        }

        var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
        if (!checkPassword.Succeeded)
        {
            _logger.LogWarning("Login failed: Incorrect password for user {Email}.", model.Email);
            return Unauthorized("Invalid email or password.");
        }

        var token = await GenerateJwtToken(user);
        _logger.LogInformation("User {Email} logged in successfully.", model.Email);
        return Ok(new { token });
    }


    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        _logger.LogInformation("Generating JWT token for user: {Email}", user.Email);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("JWT Key is not configured. Set the 'JwtKey' environment variable.");
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        _logger.LogInformation("JWT token generated for user: {Email}", user.Email);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
 