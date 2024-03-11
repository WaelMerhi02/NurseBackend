using NurseApp.Models;
using BCrypt.Net;

namespace NurseApp.GlobalFunctions
{
    public class GlobalFunctions
    {
        public static string HashPassword(string password)
        {
            password = BCrypt.Net.BCrypt.HashPassword(password);
            return password;
        }
    }
}
