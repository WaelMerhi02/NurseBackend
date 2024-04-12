using Microsoft.AspNetCore.Mvc;
using NurseApp.Models;

namespace NurseApp.Interfaces
{
    public interface IUsersRepo
    {
        public Task<object> RegisterUserAsync (Users user) ;
        public Task<bool> CheckUsernameExistAsync(string username) ;
        public Task<bool> CheckEmailExistAsync(string email) ;
        public Task<bool> CheckPhoneNumberExistAsync(string phonenumber);
        public Task<AuthenticationResult> CheckPasswordAndUsername(string username, string passowrd);
        public Task<object> GenerateTokenAsync(string username) ;
        public Task saveadvancedinfoasync(NurseAdvancedInfo advancedInfo);
        public Task<bool> IsUserBannedAsync(string username);
        public Task<object> getusersasync(bool? IsBanned);
        public Task BanUserAsync(int id, bool isbanned);
        public Task changeuserroleasync(int id, int RoleId);
        public Task saveavailabledatesasync(NurseAvailableDates saveavailabledatesasync);
        public Task<List<string>> getavailabledatesasync(int nurseid);
        public Task<object> getavailableexpertiseasync(string? expertise,bool isclientpage);
        public Task addprofilepictureasync(int userid, string profilepicturebase64string);
        public Task<object> getuserprofilebyuseridasync(int  userid);
        public Task deleteprofilepictureasync(int id);
        public Task<object> getnurseadvancedinfoasync(int userid);






    }
}
