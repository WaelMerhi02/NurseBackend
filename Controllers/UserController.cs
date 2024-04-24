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
        [Route("uploadadvancedinfo")]
        public async Task<ActionResult> uploadadvancedinfo([FromBody] NurseAdvancedInfo nurseAdvancedInfo)
        {
            await _usersRepo.saveadvancedinfoasync(nurseAdvancedInfo);
            return Ok("Nurse Advanced Info uploaded");
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> login([FromBody] Users users)
        {
            AuthenticationResult authenticationResult = await _usersRepo.CheckPasswordAndUsername(users.Username, users.Password);

           
            if (!authenticationResult.IsLoggedIn)
            {
                return BadRequest("Incorrect Username or Password");
            }
            if (authenticationResult.IsBanned)
            {
                return BadRequest("User is Banned from Accessing the App");
            }

            var tokenstring = await _usersRepo.GenerateTokenAsync(users.Username);
            var result = new
            {
                UserId = authenticationResult.UserId,
                Token = tokenstring,
                RoleId=authenticationResult.RoleId,

            };
       
            return Ok(result) ;
        }

        [HttpGet]
        [Route("getusers")]
        public async Task<ActionResult> getusers(bool? IsBanned)
        {
            return Ok(await _usersRepo.getusersasync(IsBanned));
        }

        [HttpPost]
        [Authorize]
        [Route("test")]
        public async Task<ActionResult> Test( string username)
        {
            bool isuserbanned=await _usersRepo.IsUserBannedAsync(username);
            if (isuserbanned) { return BadRequest(); }
            return Ok();
            
            
        }
        [HttpPost]
        [Route("BanUsers")]
        public async Task<ActionResult> BanUsers(int id, bool IsBanned)
        {
            await _usersRepo.BanUserAsync(id, IsBanned);
            return Ok();
        }
        [HttpPost]
        [Route("changeUserRole")]
        public async Task<ActionResult> changeuserroleasync(int id, int RoleId)
        {
            await _usersRepo.changeuserroleasync(id, RoleId);
            return Ok();
        }

        [HttpPost]
        [Route("saveavailabledates")]
        public async Task<ActionResult> saveavailabledates([FromBody] NurseAvailableDates nurseAvailableDates)
        {
            await _usersRepo.saveavailabledatesasync(nurseAvailableDates);
            return Ok();
        }

        [HttpGet]
        [Route("getavailabledates")]
        public async Task<ActionResult> getavailabledates(int nurseid)
        {
            return Ok(await _usersRepo.getavailabledatesasync(nurseid));
        }

        [HttpGet]
        [Route("getavailableexpertise")]
        public async Task<ActionResult> getavailableexpertise(string? expertise,bool isclientpage)
        {
            return Ok(await _usersRepo.getavailableexpertiseasync(expertise,isclientpage));
        }

        [HttpGet]
        [Route("getuserprofilebyuserid")]
        public async Task<ActionResult> getuserprofilebyuserid(int userid)
        {
            return Ok(await _usersRepo.getuserprofilebyuseridasync(userid));
        }

        [HttpPost]
        [Route("addprofilepicture")]
        public async Task<ActionResult> addprofilepicture([FromBody] Users user)
        {
            await _usersRepo.addprofilepictureasync(user.Id,user.ProfilePicture);
            return Ok();
        }

        [HttpPost]
        [Route("Deleteprofilepicture")]
        public async Task<ActionResult> deleteprofilepicture(int userid)
        {
            await _usersRepo.deleteprofilepictureasync(userid); 
            return Ok();
        }

        [HttpGet]
        [Route("getnurseadvancedinfo")]
        public async Task<ActionResult> getnurseadvancedinfo(int nurseid)
        {
            return Ok(await _usersRepo.getnurseadvancedinfoasync(nurseid));
        }

        [HttpGet]
        [Route("getnursesignupadvancedinfo")]
        public async Task<ActionResult> getnursesignupadvancedinfo(int userid)
        {
            return Ok(await _usersRepo.getnursesignupadvancedinfoasync(userid));
        }

        




    }
}
