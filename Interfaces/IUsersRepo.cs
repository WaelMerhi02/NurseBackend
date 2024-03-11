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
        public Task<bool> CheckPasswordAndUsername(string username, string passowrd);
        public Task<object> GenerateTokenAsync(string username) ;
    }
}
