## Implementing Authentication and Authorization
- Install packages
```
Microsoft.AspNetCore.Authentication.JwtBearer (Provides middleware to authenticate users using Jwt token)
Microsoft.IdentityModel.Tokens (To provide token validation parameters. Works with the above package.)
System.IdentityModel.Tokens.Jwt (Utilities for creating, signing and validating JWT, SAML tokens. Integrates with the above package to provide validation of token when request comes in)
Microsoft.AspNetCore.Identity.EntityFrameworkCore (Provides integration of ASP.NET Core Identity with Entity Framework Core for user and role management)
```
- Add the following to `appsettings.Development.json` or `appsettings.json`
```json
{
    "Jwt": {
        "Key": "secretkeyshouldbeatleast32byteslong",
        "Issuer": "https://localhost:7003/",
        "Audience": "https://localhost:7003/",
    }
}
```
- In `Program.cs` add the following
```cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
```
```cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        AuthenticationType = "Jwt", 
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidAudience = builder.Configuration["Jwt:Audience"],
        // or try this if the above does not work
        // ValidAudiences = new[] { builder.Configuration["Jwt:Audience"] }

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
```
- Add Authentication middleware before the authorization middleware.
```cs
app.UseAuthentication();
app.UseAuthorization();
```
- Add the `Authorize` attribute to the controller in `WorkshopsController.cs`
```cs
using Microsoft.AspNetCore.Authorization;
```
```cs
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WorkshopsController : ControllerBase
{
...
}
```
- You cannot access the Workshops resource endpoints now - you get a 401 response (UnAuthorized, but actually stands for unauthenticated request, i.e. request without valid credentials/token)
- Add connection string in `appsettings.Development.json` for Auth DB (if using MySQL. Ignore if using SQLite)
```json
"ConnectionStrings": {
    "MySQLConnection": "Server=127.0.0.1;Database=WorkshopsDB;User=root;Password=Password123#;Port=3306;",
    "MySQLAuthConnection": "Server=127.0.0.1;Database=WorkshopsAuthDB;User=root;Password=Password123#;Port=3306;"
  },
```
- Create a class under `Data/ApplicationAuthDbContext.cs`. Make sure to use the generic DbContextOptions<> in this file as well as in `Data/ApplicationDbContext.cs`
```cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AscendionAPI.Data;

public class ApplicationAuthDbContext : IdentityDbContext
{
    public ApplicationAuthDbContext(DbContextOptions<ApplicationAuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "a71a55d6-99d7-4123-b4e0-1218ecb90e3e";
        var writerRoleId = "c309fa92-2123-47be-b397-a1c77adb502c";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper()
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper()
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}
```
- In `Data/ApplicationDbContext.cs`
```cs
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}
```
- If using MySQL, configure the options for the new `ApplicationAuthDbContext` in `Program.cs`
```cs
// Configure DB service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySQLConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection"))
    ));

// Add this
builder.Services.AddDbContext<ApplicationAuthDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySQLAuthConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLAuthConnection"))
    ));
```
- If using SQLite, configure instead like so
```cs
// Configure DB service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Add this
builder.Services.AddDbContext<ApplicationAuthDbContext>(options =>
    options.UseSqlite("Data Source=auth.db"));
```
- __Note__: Generate Guids by running `Guid.NewGuid()` from the C# interactive Window (opened using View -> Other Windows -> C# interactive on Windows, and opening a terminal and running `csharp` on Mac OSX)
- Add migration and update the database.
- __IMPORTANT__: Since there are 2 DB Contexts now, you need to specify which one to use henceforth when performing migrations (for both DB Contexts).
On Windows,
```
Add-Migration "Auth DB initial migration" -Context "ApplicationAuthDbContext"
update-database -Context "ApplicationAuthDbContext"
```
On Mac,
```
Add-Migration "Auth DB initial migration" -Context "ApplicationAuthDbContext"
update-database -Context "ApplicationAuthDbContext"
```
dotnet ef migrations add "Auth DB initial migration" --context "ApplicationAuthDbContext"
dotnet ef database update --verbose --context "ApplicationAuthDbContext"
```
- Set up identity service in `Program.cs`
```cs
using Microsoft.AspNetCore.Identity;
```
```cs
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("AscendionAPI")
    .AddEntityFrameworkStores<ApplicationAuthDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});
```
- Create endpoint for registration
- Add `Controllers/AuthController.cs`
```cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AscendionAPI.Models.DTO;

namespace AscendionAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;

    public AuthController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }

    // POST: /api/Auth/Register
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };

        var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            // Add roles to this User
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                if (identityResult.Succeeded)
                {
                    return Ok("User was registered! Please login.");
                }
            }
        }

        return BadRequest("Something went wrong");
    }
}
```
- Add `RegisterRequestDto.cs`
```cs
using System.ComponentModel.DataAnnotations;

namespace AscendionAPI.Models.DTO;

public class RegisterRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string[] Roles { get; set; }
}
```
- We should now be able to register user with one of the 2 supported roles ("Reader" / "Writer"). Sample request body below.
```
{
  "username": "john.doe@example.com",
  "password": "Password123#",
  "roles": [
    "Reader"
  ]
}
```
- Create an `Repositories/ITokenRepository.cs`
```cs
using Microsoft.AspNetCore.Identity;

namespace AscendionAPI.Repositories;

public interface ITokenRepository
{
    string CreateJWTToken(IdentityUser user, List<string> roles);
}
```
- Create an `Repositories/JwtTokenRepository.cs`
```cs
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AscendionAPI.Repositories;

public class JwtTokenRepository : ITokenRepository
{
    private readonly IConfiguration configuration;

    public JwtTokenRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }


    public string CreateJWTToken(IdentityUser user, List<string> roles)
    {
        // Create claims
        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Email, user.Email));

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```
- Make the token available to the app in configuring it as a service in `Program.cs`
```cs
builder.Services.AddScoped<ITokenRepository, JwtTokenRepository>();
```
- Inject token repository instance in `Controllers/AuthController.cs`
```cs
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }

    // more code...
    // ...
}
```
- Create endpoint for login within `Controllers/AuthController.cs`
```cs
// POST: /api/Auth/Login
[HttpPost]
[Route("Login")]
public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
{
    var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

    if (user != null)
    {
        var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (checkPasswordResult)
        {
            // Get Roles for this user
            var roles = await userManager.GetRolesAsync(user);

            if (roles != null)
            {
                // Create Token

                var authToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                var response = new LoginResponseDto
                {
                    AuthToken = authToken,
                    Email = user.Email,
                    Role = roles[0]
                };

                return Ok(response);
            }
        }
    }

    return BadRequest("Username or password incorrect");
}
```
- Add `LoginRequestDto.cs`
```cs
using System.ComponentModel.DataAnnotations;

namespace AscendionAPI.Models.DTO;

public class LoginRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }


    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
```
- Add `LoginResponseDto.cs`
```cs
namespace AscendionAPI.Models.DTO;

public class LoginResponseDto
{
    public string AuthToken { get; set; }
    public string? Email { get; set; }
    public string Role { get; set; }
}
```
- Login to get a token. Make a request to get Workshops through Postman, passing `Authorization` header as `Bearer <<token>>`.
- To implement authorization, remove the `[Authorize]` attribute from the `WorkshopsController` and add it to individual action methods. For example,
```cs
[Authorize(Roles = "Reader,Writer")]
public async Task<IActionResult> GetById([FromRoute] int id)
{
    // your code...
    // ...
}
```
or,
```cs
[Authorize(Roles = "Writer")]
public async Task<IActionResult> Create([FromBody] AddWorkshopRequestDto addWorkshopRequestDto)
{
    // your code...
    // ...
}
```
- Support authorization feature in Swagger. Make these changes in `Program.cs`.
```cs
using Microsoft.OpenApi.Models;
```
```cs
builder.Services.AddEndpointsApiExplorer();

// Mody the AddSwaggerGen() call like so...
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AscendionAPI", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
```
- You can now set the Authorization header using the newly added "Authorize" button (appears on the top right of the Swagger UI documentation page for the app).

## Database Migration commands
- First setup `Data/ApplicationDbContext`, then the connection string in `appsettings.Development.json`.
- From NuGet console run this to create the database
```
update-database
```
On Mac run this instead from the terminal opened in the project folder
```
dotnet ef database update --verbose
```
- If EntityFramework Tools are not installed on Mac, you can do so using
```
dotnet tool install --global dotnet-ef
```
- Make sure the tools path is in the system PATH.
- Next add `public DBSet<Category> { get; set; }` Category to `ApplicationDbContext`.
- Migrate the table
    - On Windows run from the NuGet console
    ```
    Add-Migration "Initial Migration"
    ```
    - On Mac run this instead
    ```
    dotnet ef migrations add InitialMigration
    ```
- You will find a `Migrations` folder created with some files. This helps set up the DB on a another environment when the app is deployed.
- Now update the database once again
```
dotnet ef database update --verbose
```