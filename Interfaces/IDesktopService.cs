using System.Threading.Tasks;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.SendModel;

namespace ListaccFinance.API.Interfaces
{
    public interface IDesktopService
    {
        Task<DesktopClient> CreateDesktopClientAsync(DesktopCreateModel d);
    }
}