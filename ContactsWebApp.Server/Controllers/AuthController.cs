using ContactsWebApp.Server.Models;
using ContactsWebApp.Server.Services.Abstract;
using ContactsWebApp.Server.Utils;
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
        public IActionResult Register(RegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto.Email == null || registerRequestDto.Name == null || registerRequestDto.Surname == null || registerRequestDto.Password == null)
            {
                return BadRequest(new ApiResponse(false, "Validation Error"));
            }

            var user = new User
            {
                Name = registerRequestDto.Name,
                Surname = registerRequestDto.Surname,
                Email = registerRequestDto.Email,
                Password = registerRequestDto.Password
            };

            if (_userService.CreateUser(user))
            {
                return Ok(new ApiResponse(true, "Registiration succesful"));
            }

            return BadRequest(new ApiResponse(false, "Server error"));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
