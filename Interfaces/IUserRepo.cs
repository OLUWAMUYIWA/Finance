using System.Threading.Tasks;
using ListaccFinance.API.Data.Model;

namespace ListaccFinance.API.Interfaces
{
    public interface IUserRepo
    {
        Task<User> GertUserById(int Id);
    }
}