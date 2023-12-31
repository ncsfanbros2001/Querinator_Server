﻿using Application.HelperMethods;
using Data;
using JWT_Demo.HelperMethods;
using Microsoft.AspNetCore.Authorization;
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
        private readonly OperatorDbContext _db;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, OperatorDbContext db)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _db = db;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Login([FromBody]LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return BadRequest("Incorrect Credentials");
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
                return BadRequest("Incorrect Credentials");
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
                await _userManager.AddToRoleAsync(user, registerDTO.Role);

                PersonalConnection personalConnection = new()
                {
                    serverName = null,
                    databaseName = null,
                    username = null,
                    password = null,
                    belongsTo = user.Id
                };

                
                _db.PersonalConnections.Add(personalConnection);
                await _db.SaveChangesAsync();

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


        [HttpGet("lockUnlock/{userId}")]
        [Authorize(Roles = Statics.AdminRole)]
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
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("getUsers")]
        [Authorize(Roles = Statics.AdminRole)]
        public async Task<ActionResult> GetAllUsers()
        {
            return Ok(await _userManager.GetUsersInRoleAsync(Statics.UserRole));
        }


        [HttpPut("changePassword")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDTO.UserId);

            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.CheckPasswordAsync(user, changePasswordDTO.OldPassword) == false)
            {
                return BadRequest("Incorrect old password");
            }

            if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmNewPassword)
            {
                return BadRequest("Confirm password must be the same as the new password");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword,
                changePasswordDTO.NewPassword);

            if (result.Succeeded)
            {
                return Ok("Password changed successfully");
            }
            else
            {
                return BadRequest("Password changed failed");
            }
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
