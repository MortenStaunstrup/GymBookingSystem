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
            return BadRequest();
        result.Password = "Placeholder";
        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(User user)
    {
        Console.WriteLine($"Registering new user: {user.Email} controller");
        var result = await _userRepository.RegisterAsync(user);
        if (result == 2)
        {
            return Created();
        } 
        if (result == 1)
        {
            return BadRequest();
        }
        
        return Conflict();
        
    }

    [HttpGet]
    [Route("maxid")]
    public async Task<int> MaxId()
    {
        return await _userRepository.GetMaxId();
    }
    
    
}