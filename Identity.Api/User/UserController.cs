using System.Linq;
using System.Threading.Tasks;
using Identity.Api.Database;
using Identity.Api.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.User;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly JwtAuthenticationManager authManager;
    private readonly DatabaseContext context;

    public UserController(DatabaseContext context, JwtAuthenticationManager authManager)
    {
        this.context = context;
        this.authManager = authManager;
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        return new JsonResult(context.Users);
    }

    [HttpPost("login")]
    public IActionResult Authanticate([FromBody] UserEntity user)
    {
        UserEntity? entity =
            context.Users.FirstOrDefault(
                userEntity => userEntity.Username == user.Username && user.Password == userEntity.Password);

        if (entity == null)
        {
            return Unauthorized("Invalid Credentials!");
        }

        return new JsonResult(authManager.Authanticate(entity.Id, entity.Username));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserEntity newUserEntity)
    {
        UserEntity? existingEntity = context.Users.FirstOrDefault(entity => entity.Username == newUserEntity.Username);

        if (existingEntity != null)
        {
            return Conflict("Username already exists");
        }

        context.Users.Add(newUserEntity);
        await context.SaveChangesAsync();

        return new JsonResult(authManager.Authanticate(newUserEntity.Id, newUserEntity.Username));
    }
}