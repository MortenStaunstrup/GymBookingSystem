using Core;
using GymAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymAPI.Controllers;

[ApiController]
[Route("api/memberships")]
public class MemberShipController : ControllerBase
{
    IMemberShipRepository _membershipRepository;

    public MemberShipController(IMemberShipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result =  await _membershipRepository.GetAllAsync();
        if(result == null)
            return Ok(new List<MemberShip>());
        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddMemberShipAsync(MemberShip memberShip)
    {
        var result = await _membershipRepository.AddMemberShipAsync(memberShip);
        if(result == 1)
            return Created();
        
        return Conflict();
        
    }

    [HttpGet]
    [Route("membership/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _membershipRepository.GetByIdAsync(id);
        if(result == null)
            return NotFound();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("maxid")]
    public async Task<int> GetMaxId()
    {
        return await _membershipRepository.GetMaxIdAsync();
    }
    
}