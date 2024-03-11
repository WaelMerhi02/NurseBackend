using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NurseApp.Interfaces;
using NurseApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace NurseApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersRepo _usersRepo;

        public UserController(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> registeruser([FromBody] Users users)
        {
            if (await _usersRepo.CheckUsernameExistAsync(users.Username))
            {
                return BadRequest("Username Already Exists");
            }
            if (await _usersRepo.CheckEmailExistAsync(users.Email))
            {
                return BadRequest("Email Already Exists");
            }
            if (await _usersRepo.CheckPhoneNumberExistAsync(users.PhoneNumber))
            {
                return BadRequest("Phone Number Already Exists");
            }
            return Ok(await _usersRepo.RegisterUserAsync(users));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> login([FromBody] Users users)
        {
            if (!await _usersRepo.CheckPasswordAndUsername(users.Username, users.Password))
            {
                return Ok("Incorrect Username or Password");
            }
            var tokenstring = await _usersRepo.GenerateTokenAsync(users.Username);
       
            return Ok(tokenstring) ;
        }

        [HttpGet]
        [Authorize]
        [Route("test")]
        public async Task<ActionResult> Test()
        {
            return Ok();
        }
    }
}
