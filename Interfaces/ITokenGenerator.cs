using System.Threading.Tasks;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.SendModel;

namespace ListaccFinance.API.Interfaces
{

    public interface ITokenGenerator
    {
        Task<string> GenerateToken(DesktopClient i, int userId, string type);

        Task<string> GenerateToken(UserLogin u, int ID, string type);
        //Task<string> GenerateRefresh(string RefreshToken);
    }
}