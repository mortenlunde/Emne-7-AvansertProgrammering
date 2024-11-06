using JwtAuthorization.Models;
using JwtAuthorization.Models.Interfaces;
using JwtAuthorization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthorization.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthenticationController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    [HttpGet("[action]")]
    public async Task<ActionResult<string>> Login(string username, string password)
    {
       IIdentityUser user = await _tokenService.LoginAsync(username, password);
       string? token = await _tokenService.GenerateJwtTokenAsync(user);
       
       return Ok(token);
    }
}