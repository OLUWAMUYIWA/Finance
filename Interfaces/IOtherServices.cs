using System.Collections.Generic;
using System.Threading.Tasks;
using ListaccFinance.API.Data.ViewModel;

namespace ListaccFinance.API.Interfaces
{
    public interface IOtherServices
    {
        string Strip(string type);
        Task<List<DeptView>> ReturnDepts();
    }
}