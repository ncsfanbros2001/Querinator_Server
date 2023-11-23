using AutoMapper;
using JWT_Demo.Data;
using JWT_Demo.HelperMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entity;

namespace JWT_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly OperatorDbContext _db;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, 
            RoleManager<IdentityRole> roleManager, OperatorDbContext db)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _db = db;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Login([FromBody]LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return BadRequest("Incorrect Username or Password");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (result)
            {
                if (user.IsLocked == true)
                {
                    return BadRequest("Sorry but this account has been locked");
                }
                else
                {
                    return CreateUserObject(user);
                }
            }
            else
            {
                return BadRequest("Incorrect Username or Password");
            }

        }


        [HttpPost("register")]
        [Authorize(Roles = Statics.AdminRole)]
        public async Task<ActionResult<UserDTO>> Register([FromBody]RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                return BadRequest("Both Password fields must be the same");
            }

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


        [HttpGet("unauthorized")]
        [Authorize]
        public ActionResult CheckUnauthorized()
        {
            return Unauthorized();
        }


        [HttpGet("lockAndUnlock")]
        // [Authorize(Roles = Statics.AdminRole)]
        [AllowAnonymous]
        public async Task<ActionResult> LockAndUnlockAccount(string userId)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.IsLocked = !userToUpdate.IsLocked;

            var result = await _userManager.UpdateAsync(userToUpdate);

            if (result.Succeeded)
            {
                return Ok("Successfully");
            }
            else
            {
                return BadRequest("Failed");
            }
        }


        [HttpGet("getUsers")]
        // [Authorize(Roles = Statics.AdminRole)]
        [AllowAnonymous]
        public ActionResult GetAllUsers()
        {
            var userList = _userManager.Users;
            return Ok(userList);
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
    }
}
