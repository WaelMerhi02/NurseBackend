using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NurseApp.Interfaces; 
using NurseApp.Models;    
using NurseApp.GlobalFunctions;
using System.Drawing.Imaging;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;


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
                UserId = users.Id,
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
    
        public async Task<AuthenticationResult> CheckPasswordAndUsername(string username,string password)
        {
            var user =  await dbContext.Users.FirstOrDefaultAsync(u=>u.Username == username);
            if (user == null) return new AuthenticationResult { IsLoggedIn = false };
            else if(user.IsBanned) return new AuthenticationResult {IsBanned=true,IsLoggedIn=true };
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return new AuthenticationResult { IsLoggedIn = false };
                }
                else
                {
                    return new AuthenticationResult
                    {
                        IsLoggedIn = true,
                        UserId = user.Id,
                        RoleId=user.RoleId,
                        IsBanned=false
                    };
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

        public async Task saveadvancedinfoasync(NurseAdvancedInfo advancedInfo)
        {
            await saveimageinlocalfolder(advancedInfo);
            dbContext.NurseAdvancedInfo.Add(advancedInfo);
            await dbContext.SaveChangesAsync();

        }

        public async Task saveimageinlocalfolder(NurseAdvancedInfo advancedInfo)
        {
            if (!advancedInfo.passportimage.IsNullOrEmpty())
            {
                byte[] imageBytes = Convert.FromBase64String(advancedInfo.passportimage);

                
                string folderPath = @"C:\Users\Owner\Desktop\VerificationImages\PassportImages";
                string fileName = $"{advancedInfo.NurseId}.jpg";
                string fullPath = Path.Combine(folderPath, fileName);
                await File.WriteAllBytesAsync(fullPath, imageBytes);
                advancedInfo.passportimage = fullPath;
            }
            if (!advancedInfo.IdBackImage.IsNullOrEmpty())
            {
                byte[] imageBytes = Convert.FromBase64String(advancedInfo.IdBackImage);


                string folderPath = @"C:\Users\Owner\Desktop\VerificationImages\IdBackImage";
                string fileName = $"{advancedInfo.NurseId}.jpg";
                string fullPath = Path.Combine(folderPath, fileName);
                await File.WriteAllBytesAsync(fullPath, imageBytes);
                advancedInfo.IdBackImage = fullPath;
            }
            if (!advancedInfo.IdFrontImage.IsNullOrEmpty())
            {
                byte[] imageBytes = Convert.FromBase64String(advancedInfo.IdFrontImage);


                string folderPath = @"C:\Users\Owner\Desktop\VerificationImages\IdFrontImage";
                string fileName = $"{advancedInfo.NurseId}.jpg";
                string fullPath = Path.Combine(folderPath, fileName);
                await File.WriteAllBytesAsync(fullPath, imageBytes);
                advancedInfo.IdFrontImage = fullPath;
                
            }


        }
        public async Task <bool>  IsUserBannedAsync(string username)
        {
            bool IsUserBanned = await dbContext.Users.AnyAsync(u => u.Username == username && u.IsBanned == true);
            return IsUserBanned;
        }
        public async Task<object> getusersasync(bool? IsBanned)
        {
            if(IsBanned == null)
            {
                return await dbContext.Users.ToListAsync();
            }
            if ((bool)IsBanned)
            {
                return await dbContext.Users.Where(x=>x.IsBanned==true).ToListAsync();
            }
            if ((bool)!IsBanned)
            {
                return await dbContext.Users.Where(x=>x.IsBanned==false).ToListAsync();
            }
            return null;
            
        }
        public async Task BanUserAsync(int id,bool isbanned)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.IsBanned= isbanned;
            await dbContext.SaveChangesAsync();
        }
        public async Task changeuserroleasync(int id,int RoleId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.RoleId = RoleId;
            await dbContext.SaveChangesAsync();
        }

        public async Task saveavailabledatesasync(NurseAvailableDates nurseAvailableDates)
        {
            var nurse = await dbContext.NurseAvailableDates.FirstOrDefaultAsync(u => u.NurseId == nurseAvailableDates.NurseId);
            if (nurse == null)
            {
                dbContext.NurseAvailableDates.Add(nurseAvailableDates);
                await dbContext.SaveChangesAsync();

            }
            else
            {
                nurse.AvailableDates = nurseAvailableDates.AvailableDates;
                await dbContext.SaveChangesAsync();

            }

        }
        public async Task<List<string>> getavailabledatesasync(int nurseid)
        {
            var nurse = await dbContext.NurseAvailableDates.FirstOrDefaultAsync(u => u.NurseId == nurseid);
            if (nurse == null || nurse.AvailableDates.IsNullOrEmpty())
            {
                return [];
            }
            else
            {
                List<string> result = nurse.AvailableDates.Split(",").ToList();
                return result;
            }

        }

        public async Task<object>  getavailableexpertiseasync(string? expertise,bool isclientpages)
        {
            var nurses = await dbContext.Users.Join(dbContext.NurseAdvancedInfo,
                users => users.Id,
                nurseadvancedinfo => nurseadvancedinfo.NurseId,
                (user, nurseadvancedinfo) => new NurseBasicInfo
                {
                    NurseId = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Expertise = nurseadvancedinfo.Expertise,
                    Location = nurseadvancedinfo.Location,
                    IsBanned = user.IsBanned,
                    Profile = user.ProfilePicture


                }).Where(u=>(expertise=="All"?u.Expertise==u.Expertise:u.Expertise==expertise) && u.IsBanned==false).ToListAsync();

                foreach(var nurse in nurses)
                {
                    nurse.Profile = await getprofilepicture(nurse.Profile);
                }
                return isclientpages==false?nurses:nurses.Take(3);
             
        }

        public async Task<object> getuserprofilebyuseridasync(int userid)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userid);
            return new
            {
                email = user.Email,
                phonenumber = user.PhoneNumber,
                username = user.Username,
                firstname = user.FirstName,
                lastname = user.LastName,
                profile = await getprofilepicture(user.ProfilePicture)
                
                
            };

        }

        public async Task addprofilepictureasync(int userid,string profilepicturebase64string)
        {
            var user=await dbContext.Users.FirstOrDefaultAsync(u=>u.Id == userid);
                byte[] imageBytes = Convert.FromBase64String(profilepicturebase64string);


                string folderPath = @"C:\Users\Owner\Desktop\VerificationImages\ProfilePictures";
                string fileName = $"{userid}.jpg";
                string fullPath = Path.Combine(folderPath, fileName);
                await File.WriteAllBytesAsync(fullPath, imageBytes);
                user.ProfilePicture = fullPath;
                await dbContext.SaveChangesAsync();
            
        }

        public async Task deleteprofilepictureasync(int userid)
        {
            var user= await dbContext.Users.FirstOrDefaultAsync(u=>u.Id==userid);
            if (File.Exists(user.ProfilePicture))
            {
                File.Delete(user.ProfilePicture);
            }
            user.ProfilePicture= null;
            await dbContext.SaveChangesAsync();
        }

        public async Task<string?> getprofilepicture(string? profile)
        {
            if (profile == null) return null;
            byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(profile);
            string base64Image = Convert.ToBase64String(imageBytes);
            return base64Image;
        }

        public async Task<object> getnurseadvancedinfoasync(int userid)
        {
            var userIdParam = new SqlParameter("@UserId", userid);
            NurseDetails result = dbContext.NurseDetails.FromSqlRaw("EXEC GetNurseDetails @UserId", userIdParam).ToList()[0];
            result.ProfilePicture= await getprofilepicture(result.ProfilePicture);
            return result;

        }











    }
}
