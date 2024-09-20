using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            try
            {
                var appUser = new AppUser
                {
                    UserName = signUpDto.Username,
                    Email = signUpDto.Email,
                };
                var createdUser = await _userManager.CreateAsync(appUser, signUpDto.Password);
                if (!createdUser.Succeeded)
                {
                    return StatusCode(500, createdUser.Errors);
                }
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, roleResult.Errors);
                }
                var response = new NewUserDto
                {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Token = _tokenService.CreateToken(appUser),
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.StackTrace);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == signInDto.Username);
            if (user == null) return Unauthorized("Invalid Username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, signInDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username and Password not matching");

            return Ok(new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
            });
        }
    }
}