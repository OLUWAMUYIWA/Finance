using System.Threading.Tasks;
using ListaccFinance.Api.Data;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Interfaces;
using ListaccFinance.API.SendModel;

namespace ListaccFinance.API.Services

{
    public class DesktopService : IDesktopService
    {

        private readonly DataContext _context;

        public DesktopService(DataContext context)
        {
            _context = context;
        }
        public async Task<DesktopClient> CreateDesktopClientAsync(DesktopCreateModel d)
        {
            var newD = new DesktopClient(){
                ClientName = d.ClientName,
                ClientMacAddress = d.ClientMacAddress,
                ClientType = d.ClientType,
            };
            var newDc = await _context.DesktopClients.AddAsync(newD);
            await _context.SaveChangesAsync();
            var result =  newDc.Entity;
            return result;
        }

    }
}