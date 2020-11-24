using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ListaccFinance.Api.Data;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Data.ViewModel;
using ListaccFinance.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ListaccFinance.API.Services
{
    public class OtherServices : IOtherServices
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OtherServices(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public string Strip(string type)
        {
            var reType =  type.Substring(type.LastIndexOf('.')+1);
            return reType;
        }

        public async Task<List<DeptView>> ReturnDepts()
        {
            var x = await _context.Set<Department>().ToListAsync();
            var depts = _mapper.Map<List<DeptView>>(x);
            return  depts;
        }
    }
}