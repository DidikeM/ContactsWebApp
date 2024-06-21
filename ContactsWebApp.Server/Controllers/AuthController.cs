using ContactsWebApp.Server.JwtFeatures;
using ContactsWebApp.Server.Services.Abstract;
using ContactsWebApp.Shared.DTOs;
using ContactsWebApp.Shared.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ContactsWebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(JwtHandler jwtHandler, IUserService userService) : ControllerBase
    {
        private readonly JwtHandler _jwtHandler = jwtHandler;
        private readonly IUserService _userService = userService;

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto loginRequestDto)
        {
            var user = _userService.GetUserByEmail(loginRequestDto.Email);
            if (user == null || !_userService.CheckPassword(user, loginRequestDto.Password))
                return Unauthorized(new ApiResponse(false, "Invalid Authentication"));

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new ApiResponse<LoginResponseDto>(true, "", new LoginResponseDto { Token = token, User = new UserDto { Email = user.Email, Name = user.Name, Surname = user.Surname } }));
        }

        [HttpPost("register")]
        public IActionResult Register()
        {
            return Ok();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
