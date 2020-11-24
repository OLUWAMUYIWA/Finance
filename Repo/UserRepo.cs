using System.Collections.Generic;
using ListaccFinance.Api.Data;
using System.Linq;
using ListaccFinance.API.Data.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ListaccFinance.API.Interfaces;

namespace ListaccFinance.Api.Repo

{
    public class UserRepo: IUserRepo
    {
        private readonly DataContext _context;
        public UserRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GertUserById(int Id) 
        {
           var u = await _context.Users.Include(x => x.Person).Include(x => x.Department).Where(x => x.Id == Id).FirstOrDefaultAsync();
           return u;
        }
    }
}