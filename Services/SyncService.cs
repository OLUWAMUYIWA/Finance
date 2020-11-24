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

    public class SyncService : ISyncService
    {
        
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        private readonly IUserService _uService;

        public SyncService (DataContext context, IMapper mapper, IUserService uService)
        {
            _context = context;
            _mapper = mapper;
            _uService = uService;
        }


       /* public async Task<string> UploadAsync<T> (T syncV, Department d) where T : class
        {

            var props = syncV.GetType().GetProperties();

            foreach (var prop in props)
            {
                switch (prop.GetType().ToString())
                {
                    case "Department":
                        await AddAsync<Department>(d);
                        break;
                }
            }
            
            return "successful";
        }*/

        public async Task<SavedList> UploadDeptAsync(Department d, int OffId) 
        {
                try
                {
                    //int deptId = d.Id;
                    await _context.Departments.AddAsync(d);
                    
                    await _context.SaveChangesAsync();
                    var newSaved = new SavedList
                    {
                        Id = OffId,
                        Table = "Departments",
                        OnlineEntryId = d.Id,
                    };
                    return newSaved;
                }
                catch (System.Exception e)
                {

                    throw e;
                }
        }
        //If onlineEntryid is null
        public async Task<int> UploadOldDeptAsync(Department d, int OnlineId)
        {
            var thisDept = await _context.Departments.Where(x => x.Id == OnlineId).FirstOrDefaultAsync();
            thisDept.Name = d.Name;
            await _context.SaveChangesAsync();
            return thisDept.Id;


        }
        public async Task<SavedList> UploadPersonAsync(Person p, int OffId)
        {

            try
            {
                //int perId = p.Id;
                await _context.Persons.AddAsync(p);
                await _context.SaveChangesAsync();
                var newSaved = new SavedList{
                    Id = OffId,
                    Table = "Persons",
                    OnlineEntryId = p.Id
                };
                return newSaved;
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        public async Task<int> UploadOldPersonAsync(Person p, int OnlineId)
        {
            var thisPerson = await _context.Persons.FindAsync(OnlineId);
            thisPerson.DateOfBirth = p.DateOfBirth;
            thisPerson.firstName = p.firstName;
            thisPerson.LastName = p.LastName;
            await _context.SaveChangesAsync();
            return thisPerson.Id;
        }
       
        public async Task<SavedList> UploadClientAsync(Client c, int OffId)
        {
            try
            {
                if (!string.IsNullOrEmpty(c.BusinessName))
                {
                    var biz = new Client();
                    biz.Address = c.Address;
                    biz.AmountReceivable = c.AmountReceivable;
                    biz.BusinessName = c.BusinessName;
                    biz.Email = c.Email;
                    biz.Phone = c.Phone;
                    biz.UId = c.UId;
                    biz.UId2 = c.UId2;

                    await _context.Clients.AddAsync(biz);
                    await _context.SaveChangesAsync();

                    return new SavedList
                    {
                        Id = OffId,
                        Table = "Clients",
                        OnlineEntryId = biz.Id,
                    };

                }

                var newC = new Client();
                var per = new Person{
                    firstName = c.Person.firstName,
                    LastName = c.Person.LastName,
                    Gender = c.Person.Gender,
                    DateOfBirth = c.Person.DateOfBirth,
                };
                newC.Address = c.Address;
                newC.AmountReceivable = c.AmountReceivable;
               // newC.BusinessName = c.BusinessName;
                newC.Email = c.Email;
                newC.PersonId = c.PersonId;
                newC.Phone = c.Phone;
                newC.UId = c.UId;
                newC.UId2 = c.UId2;
                newC.Person = per;

                await _context.Clients.AddAsync(newC);
                await _context.SaveChangesAsync();
                
                return new SavedList{
                    Id = OffId,
                    Table = "Clients",
                    OnlineEntryId = newC.Id,
                };
            }
            catch (System.Exception e)
            {
                
                throw e;
            }
        }
        public async Task<int> UploadOldClientAsync(Client c, int OnlineId)
        {
            var thisClient =await _context.Clients.Where(x => x.Id == OnlineId).Include(x => x.Person).FirstOrDefaultAsync();
            thisClient.Address = c.Address;
            thisClient.AmountReceivable = c.AmountReceivable;
            thisClient.BusinessName = c.BusinessName;
            thisClient.Email = c.Email;
            thisClient.Phone = c.Phone;
            thisClient.UId = c.UId;
            thisClient.UId2 = c.UId2;
            
            thisClient.Person.DateOfBirth = c.Person.DateOfBirth;
            thisClient.Person.LastName = c.Person.LastName;
            thisClient.Person.firstName = c.Person.firstName;
            thisClient.Person.Gender = c.Person.Gender;
            await _context.SaveChangesAsync();
            return thisClient.Id;
        }

        public async Task<SavedList> UploadProjectAsync(Project p, int OldId)
        {
            try
            {

                //int prId = p.Id;
                await _context.Projects.AddAsync(p);
                await _context.SaveChangesAsync();
                return new SavedList{
                    Id = OldId,
                    Table = "Projects",
                    OnlineEntryId = p.Id,
                };
                
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        public async Task<int> UploadOldProjectAsync(Project p, int OnlineId)
        {
            var thisProject = await _context.Projects.FindAsync(OnlineId);
            thisProject.DepartmentId = p.DepartmentId;
            thisProject.Description = p.Description;
            thisProject.Name = p.Name;
            await _context.SaveChangesAsync();
            return thisProject.Id;
        }

        public async Task<SavedList> UploadCostAsync(CostCategory c, int OldId)
        {
            try
            {
                //int ccId = c.Id;
                await _context.CostCategories.AddAsync(c);
                await _context.SaveChangesAsync();
                return new SavedList{
                    Id = OldId,
                    Table = "CostCategories",
                    OnlineEntryId = c.Id
                };
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UploadOldCostAsync(CostCategory c, int OnlineId)
        {
            var thisCost = await _context.CostCategories.FindAsync(OnlineId);
            thisCost.Description = c.Description;
            thisCost.Name = c.Name;
            thisCost.Type = c.Type;
            await _context.SaveChangesAsync();
            return thisCost.Id;
        }
        public async Task<SavedList> UploadExpenditureAsync(Expenditure e, int OldId)
        {
            try
            {
                //int exId = e.Id;
                await _context.Expenditures.AddAsync(e);
                await _context.SaveChangesAsync();
                return new SavedList{
                    Id = OldId,
                    Table = "Expenditures",
                    OnlineEntryId = e.Id
                };
            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }
        public async Task<int> UploadOldExpenditureAsync(Expenditure e, int OnlineId)
        {
            var thisExp  =await _context.Expenditures.FindAsync(OnlineId);
            thisExp.Amount = e.Amount;
            thisExp.ClientId = e.ClientId;
            thisExp.CostCategoryId = e.CostCategoryId;
            thisExp.ProjectId = e.ProjectId;
            thisExp.Date = e.Date;
            thisExp.Description = e.Description;
            thisExp.IssuerId = e.IssuerId;
            await _context.SaveChangesAsync();
            return thisExp.Id;
        }
        public async Task<SavedList> UploadServiceAsync(Service s, int OldId)
        {
            try
            {
                //int sId = s.Id;
                await _context.Services.AddAsync(s);
                await _context.SaveChangesAsync();

                return new SavedList{
                    Id = OldId,
                    Table = "Services",
                    OnlineEntryId = s.Id

                };
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        public async Task<int> UploadOldServiceAsync(Service s, int OnlineId)
        {
            var thisServ  = await _context.Services.FindAsync(OnlineId);
            thisServ.Amount = s.Amount;
            thisServ.Description = s.Description;
            thisServ.FixedAmount = s.FixedAmount;
            thisServ.Name = s.Name;
            thisServ.ProjectId = s.ProjectId;
            await _context.SaveChangesAsync();
            return thisServ.Id;
        }
        public async Task<SavedList> UploadIncomeAsync(Income i, int OldId)
        {
            try
            {
                //int iId = i.Id;
                await _context.Incomes.AddAsync(i);
                await _context.SaveChangesAsync();
                return new SavedList{
                    Id = OldId,
                    Table = "Incomes",
                    OnlineEntryId = i.Id
                };
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        public async Task<int> UploadOldIncomeAsync(Income i, int OnlineId)
        {
            var thisInc = await _context.Incomes.FindAsync(OnlineId);
            thisInc.AmountReceivable = i.AmountReceivable;
            thisInc.AmountReceived = i.AmountReceived;
            thisInc.ClientId = i.ClientId;
            thisInc.Date = i.Date;
            thisInc.DateDue = i.DateDue;
            thisInc.Discount = i.Discount;
            thisInc.IncomeId = i.IncomeId;
            thisInc.PaymentType = i.PaymentType;
            thisInc.ServiceId = i.ServiceId;
            thisInc.Type = i.Type;
            thisInc.Unit = i.Unit;
            thisInc.UserId = i.UserId;
            await _context.SaveChangesAsync();
            return thisInc.Id;
        }


        // see if there's more work to do on this later
        public async Task HandleChangeUpload(Change ch)
        {
            await _context.Changes.AddAsync(ch);
            return;
        }

        public bool IsExist<T> (int Id) where T : class
        {

            var item = _context.Set<T>().Find(Id);
            if (item is null)
            {
                return false;
            }
            return true;
        }

        public async Task<DepartMentViewModel> DownloadDeptAsync(Change ch)
        {
            var deptCh = await _context.Departments.Where(x => x.Id == ch.EntryId).FirstOrDefaultAsync();
            var dept = _mapper.Map<DepartMentViewModel>(deptCh);
            dept.ChangeId = ch.Id;
            return dept;
        }

        public async Task<PersonViewModel> DownloadPersonAsync(Change ch)
        {
            var perCh = await _context.Persons.Where(x => x.Id == ch.EntryId).FirstOrDefaultAsync();
            var person = _mapper.Map<PersonViewModel>(perCh);
            person.ChangeId = ch.Id;
            return person;
        }

        public async Task<UserViewModel> DownloadUserAsync(Change ch)
        {
            var userCh = await _context.Users.Where(x => x.Id == ch.EntryId).Include(x => x.Person).FirstOrDefaultAsync();
            var user = _mapper.Map<UserViewModel>(userCh);
            user.ChangeId = ch.Id;
            user.Discriminator = userCh.GetType().Name;
            return user;

        }

        public async Task<ClientViewModel> DownloadClientAsync(Change ch)
        {
            var clientCh = await _context.Clients.Where(x => x.Id == ch.EntryId).Include(x => x.Person).FirstOrDefaultAsync();//
            var client = _mapper.Map<ClientViewModel>(clientCh);
            client.ChangeId = ch.Id;
            return client;
        }

        public async Task<ProjectViewModel> DownloadProjectAsync (Change ch)
        {
            var projectCh = await _context.Projects.Where(x => x.Id == ch.EntryId).FirstOrDefaultAsync();
            var project = _mapper.Map<ProjectViewModel>(projectCh);
            project.ChangeId = ch.Id;
            return project;
        }

        public async Task<CostCategoryViewModel> DownloadCostAsync(Change ch)
        {
            var costCh = await _context.CostCategories.Where(x => x.Id == ch.EntryId).FirstOrDefaultAsync();
            var cost =  _mapper.Map<CostCategoryViewModel>(costCh);
            cost.ChangeId = ch.Id;
            return cost;
        }

        public async Task<ExpenditureViewModel> DownloadExpenditureAsync(Change ch)
        {
            var expCh = await _context.Expenditures.Where(x => x.Id == ch.EntryId).FirstOrDefaultAsync();
            var expenditure =  _mapper.Map<ExpenditureViewModel>(expCh);
            expenditure.ChangeId = ch.Id;
            return expenditure;
        }

        public async Task<ServiceViewModel> DownloadServicesAsync(Change ch)
        {
            var serviceCh = await _context.Services.Where(x => x.Id == ch.EntryId).FirstOrDefaultAsync();
            var service =  _mapper.Map<ServiceViewModel>(serviceCh);
            service.ChangeId = ch.Id;
            return service;
        }



        public async Task<IncomeViewModel> DownloadIncomesAsync(Change ch)
        {
            var incomeCh = await _context.Incomes.Where(x => x.Id == ch.EntryId).FirstOrDefaultAsync();
            var income = _mapper.Map<IncomeViewModel>(incomeCh);
            income.ChangeId = ch.Id;
            return income;
        }

    } 


}
