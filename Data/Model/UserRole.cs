using Microsoft.AspNetCore.Identity;

namespace ListaccFinance.API.Data.Model
{
    public class UserRole: IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
        
    }
}