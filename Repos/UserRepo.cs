using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NurseApp.Interfaces; // Assuming this is where your repository interfaces are defined
using NurseApp.Models;    // Assuming this is where your data models are defined
using NurseApp.GlobalFunctions; // Assuming this is where your global functions like HashPassword are defined


namespace NurseApp.Repos
{
    public class UserRepo:IUsersRepo
    {
        private readonly NurseAppDbContext dbContext;
        public UserRepo(NurseAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<object> RegisterUserAsync(Users users) 
        {

            users.Password =GlobalFunctions.GlobalFunctions.HashPassword(users.Password);
            dbContext.Users.Add(users);
            await dbContext.SaveChangesAsync();
            var tokenstring = await GenerateTokenAsync(users.Username);
            var result = new
            {
                Token = tokenstring
            };
            return result;




        }
        public async Task<bool> CheckUsernameExistAsync(string username)
        {
            bool usernameExists = await dbContext.Users.AnyAsync(u => u.Username == username);

            return usernameExists;
        }
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            bool emailExists = await dbContext.Users.AnyAsync(u => u.Email == email);

            return emailExists;
        }
        public async Task<bool> CheckPhoneNumberExistAsync(string phonenumber)
        {
            bool phonenumberExits = await dbContext.Users.AnyAsync(u => u.PhoneNumber == phonenumber);

            return phonenumberExits;
        }
    
        public async Task<bool> CheckPasswordAndUsername(string username,string password)
        {
            var user =  await dbContext.Users.FirstOrDefaultAsync(u=>u.Username == username);
            if (user == null) return false;
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        public async Task<object> GenerateTokenAsync(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourSuperDuperSecretKeyHereKFCHashem");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                 new Claim(ClaimTypes.Name, username)

                }),
                Expires = DateTime.UtcNow.AddMinutes(3), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;


        }


    }
}
