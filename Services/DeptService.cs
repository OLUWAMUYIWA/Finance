using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ListaccFinance.Api.Data;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Data.ViewModel;
using ListaccFinance.API.Interfaces;
using ListaccFinance.API.SendModel;
using Microsoft.EntityFrameworkCore;

namespace ListaccFinance.API.Services

{
    public class DeptService : IDeptService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeptService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /*public async Task<List<DeptView>> ReturnAllDeptViews() 
        {
            try
            {
                var x = await _context.Set<Department>().ToListAsync();
                var depts = _mapper.Map<List<DeptView>>(x);
                return depts;
            }
            catch (System.Exception)
            {
                throw;
            }

        }*/

        public async Task<List<Department>> ReturnAllDepts()
        {
            try
            {
                var x = await _context.Set<Department>().ToListAsync();
                return x;
            }
            catch (System.Exception)
            {   
                throw;
            }
        }


        public async Task CreateDepartment(string name, int MyId)
        {
            try
            {
                // save department
                var dept = new Department()
                {
                    Name = name
                };
                await _context.AddAsync<Department>(dept);
                await _context.SaveChangesAsync();

                // save change entry for department creation
                var change = new Change()
                {
                    Table = "Departments",
                    ChangeType = "Create",
                    EntryId = dept.Id,
                    OnlineTimeStamp = DateTime.Now,
                    OfflineTimeStamp = DateTime.Now,
                    UserId = MyId
                };
                await _context.Changes.AddAsync(change); ;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        
        public async Task<bool> IsDeptExist(string name)
        {
            var thisDept = await _context.Departments.Where(x => x.Name.ToLower().CompareTo(name.ToLower()) == 0).FirstOrDefaultAsync();
            if (thisDept is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> IsDeptExist(string name, int departmentId)
        {
            var thisDept = await _context.Departments.Where(x => x.Id != departmentId && x.Name.ToLower().CompareTo(name.ToLower()) == 0).FirstOrDefaultAsync();
            if (thisDept is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task EditDepartment(int Id, string newName, int MyId)
        {
            try
            {
                // save department
                var thisDept = await _context.Departments.FindAsync(Id);
                thisDept.Name = newName;
                await _context.SaveChangesAsync();

                // save change      
                var change = new Change
                {
                    Table = "Departments",
                    EntryId = thisDept.Id,
                    ChangeType = "Edit",
                    OfflineTimeStamp = DateTime.Now,
                    OnlineTimeStamp = DateTime.Now,
                    UserId = MyId
                };
                await _context.Changes.AddAsync(change);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PagedList<Department>> ReturnPagedUserList(SearchDept props)
        {

            // s.SearchString = dept name
            IQueryable<Department> depts = Enumerable.Empty<Department>().AsQueryable();
            if (string.IsNullOrEmpty(props.SearchString) || string.IsNullOrWhiteSpace(props.SearchString))
            {
                depts = _context.Set<Department>();
            }
            else
            {
                depts = depts.Concat(await _context.Departments.Where(x => x.Name.ToLower().Contains(props.SearchString.ToLower())).OrderBy(x => x.Id).ToListAsync());
            }
            
            var pagedDept = PagedList<Department>.ToPagedList(depts, props.PageNumber, props.PageSize);
            return pagedDept;     
        }
    }
}
