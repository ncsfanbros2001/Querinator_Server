using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entity;
using System.Net;
using System.Security.Claims;

namespace JWT_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Login([FromBody]LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (result)
            {
                return CreateUserObject(user);
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost("register")]
        [Authorize(Roles = Statics.AdminRole)]
        public async Task<ActionResult<UserDTO>> Register([FromBody]RegisterDTO registerDTO)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDTO.Username))
            {
                return BadRequest("This username is already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDTO.Email))
            {
                return BadRequest("This email is already taken");
            }

            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Username
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(Statics.AdminRole).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(Statics.AdminRole));
                    await _roleManager.CreateAsync(new IdentityRole(Statics.CustomerRole));
                }

                await _userManager.AddToRoleAsync(user, Statics.CustomerRole);

                return CreateUserObject(user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("currentUser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email)!);

            return CreateUserObject(user);
        }

        private UserDTO CreateUserObject(AppUser user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName!
            };
        }

        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorised()
        {
            return Unauthorized();
        }
    }
}
