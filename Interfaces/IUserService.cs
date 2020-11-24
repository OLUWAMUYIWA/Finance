using System.Collections.Generic;
using System.Threading.Tasks;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Data.ViewModel;
using ListaccFinance.API.SendModel;
using ListaccFinance.API.Services;

namespace ListaccFinance.API.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(RegisterModel reg);
        Task<string> CreateUserAsync(RegisterModel reg, int userId);
        Task EditUserAsync(int Id, RegisterModel reg, int MyId);
        bool IsUserExist();
        Task<bool> IsThisUserExist(string UserEmail);
        Task CreateAdmin(RegisterModel reg, int userId);
        Task Deactivate(int Id, int MyId);
        Task Activate(int Id, int MyId);
        Task<PagedList<User>> ReturnUsers (SearchPaging props);
        Task<PagedList<User>> ReturnAllUsers(SearchPaging props);
        Task<RegisterModel> ReturnUser(int Id);
    }
}