using Core;
using GymAPI.Repositories;
using GymAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace GymAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    
    [HttpGet]
    [Route("login/{email}/{password}")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _userRepository.TryLoginAsync(email, password);
        if (result == null)
            return NoContent();
        result.Password = "Placeholder";
        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(User user)
    {
        var result = await _userRepository.RegisterAsync(user);
        if (!result)
        {
            return BadRequest();
        }

        return Ok(true);
    }
    
    
}