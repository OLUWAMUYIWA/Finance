using Microsoft.AspNetCore.Identity;

namespace ListaccFinance.API.Data.Model
{
    public class Role: IdentityRole<int>
    {
        public static string Admin = "Admin";
    }
}