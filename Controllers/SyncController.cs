using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ListaccFinance.Api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ListaccFinance.API.Interfaces;
using ListaccFinance.API.Data.Model;
using System;
using ListaccFinance.API.Services;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using ListaccFinance.API.SendModel;
using ListaccFinance.API.Data.ViewModel;
using AutoMapper;

namespace ListaccFinance.API.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly ITokenGenerator _tokGen;

        private readonly IDesktopService _dService;
        //private readonly ISyncService<> _sservice;

        private readonly ISyncService _sservice;
        private readonly IMapper _mapper;

        public SyncController(DataContext context,
                                ITokenGenerator tokGen,
                                IDesktopService dService,
                                ISyncService sservice,
                                IMapper mapper
                             )
        {
            _context = context;
            _tokGen = tokGen;
            _dService = dService;
            _sservice = sservice;
            _mapper = mapper;

        }

        // private int deptId;


        [Authorize]
        [HttpPost("CreateDesktopClient")]
        public async Task<IActionResult> CreateDesktop(DesktopCreateModel m)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var response = await _dService.CreateDesktopClientAsync(m);
            return Ok(response);
        }





        public async Task SaveChangesAsync(DateTime ChangeTimestamp, string Change, string Table, int EntryId, int UserId)
        {
            int DesktopClientId = int.Parse(this.User.Claims.First(x => x.Type == "Desktopid").Value);
            DateTime OnlineTimeStamp = DateTime.Now;
            var newChange = new Change
            {
                DesktopClientId = DesktopClientId,
                UserId = UserId,
                OnlineTimeStamp = OnlineTimeStamp,
                OfflineTimeStamp = ChangeTimestamp,
                ChangeType = Change,
                Table = Table,
                EntryId = EntryId
            };
            await _context.Changes.AddAsync(newChange);
            await _context.SaveChangesAsync();

        }


        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadData(List<UploadSyncViewModel> lsync)
        {
            List<SavedList> mapList = new List<SavedList>();
            SavedList save;

            try
            {

                foreach (UploadSyncViewModel sync in lsync)
                {
                    switch (sync.Table)
                    {

                        case "Departments":
                            var dChange = _mapper.Map<Department>(sync.dept);
                            int dId = sync.dept.Id;

                            if (sync.dept.OnlineEntryId is null)
                            {
                                if (sync.dept.Change is "EDIT")
                                {
                                    int OnlineId = 0;
                                    foreach (var map in mapList)
                                    {
                                        if (map.Id == sync.dept.Id && map.Table.CompareTo("Departments") == 0)
                                        {
                                            OnlineId = map.OnlineEntryId;
                                            break;
                                        }
                                    }

                                    int EntryId = await _sservice.UploadOldDeptAsync(dChange, OnlineId);
                                    await SaveChangesAsync(sync.dept.ChangeTimeStamp, sync.dept.Change, sync.Table, EntryId, sync.dept.ChangeUserOnlineEntryId);

                                    break;
                                }
                                save = await _sservice.UploadDeptAsync(dChange, dId);
                                mapList.Add(save);
                                await SaveChangesAsync(sync.dept.ChangeTimeStamp, sync.dept.Change, sync.Table, save.OnlineEntryId, sync.dept.ChangeUserOnlineEntryId);
                            }
                            else
                            {
                                int EntryId = await _sservice.UploadOldDeptAsync(dChange, sync.dept.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.dept.ChangeTimeStamp, sync.dept.Change, sync.Table, EntryId, sync.dept.ChangeUserOnlineEntryId);
                            }

                            break;


                        /*case "Persons":
                            var pChange = _mapper.Map<Person>(sync.person);
                            int pId = sync.person.Id;
                            if (sync.person.OnlineEntryId is null)
                            {
                                save = await _sservice.UploadPersonAsync(pChange, pId);
                                mapList.Add(save);
                                await SaveChangesAsync(sync.person.ChangeTimeStamp, sync.person.Change, sync.Table, save.OnlineEntryId, sync.dept.ChangeUserOnlineEntryId);

                            }
                            else
                            {
                               int EntryId =  await _sservice.UploadOldPersonAsync(pChange, sync.person.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.person.ChangeTimeStamp, sync.person.Change, sync.Table, EntryId, sync.dept.ChangeUserOnlineEntryId);
                            }
                           
                            break;

                        case "Users":

                            if (sync.user.DepartmentOnlineEntryId is null)
                            {
                                foreach (var mapped in mapList)
                                {
                                    if (mapped.Id == sync.user.DepartmentId && mapped.Table == "Departments")
                                    {
                                        deptId = mapped.OnlineEntryId;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                deptId = sync.user.DepartmentId;
                            }
                            var regUser = new RegisterModel
                            {
                                firstName = sync.user.person.firstName,
                                LastName = sync.user.person.LastName,
                                Gender = sync.user.person.Gender,
                                DateOfBirth = sync.user.person.DateOfBirth,
                                Phone = sync.user.Phone,
                                Address = sync.user.Address,
                                EmailAddress = sync.user.Email,
                                Password = sync.user.Password,
                                DepartmentId = deptId
                            };
                            int uId = sync.user.Id;
                            await _sservice.UploadUserAsync(regUser, sync.user.Id);
                            await SaveChangesAsync(sync.user.ChangeTimeStamp, sync.user.Change, sync.Table, uId, sync.user.ChangeUserOnlineEntryId);
                            break;*/


                        case "Clients":
                            var cChange = _mapper.Map<Client>(sync.client);
                            int cId = sync.client.Id;
                            if (sync.client.OnlineEntryId is null)
                            {
                                if (sync.client.Change.CompareTo("EDIT") == 0)
                                {
                                    int OnlineId = 0;
                                    foreach (var map in mapList)
                                    {
                                        if (map.Id == sync.client.Id && map.Table.CompareTo("Clients") == 0)
                                        {
                                            OnlineId = map.OnlineEntryId;
                                            break;
                                        }
                                    }
                                    int EntryId = await _sservice.UploadOldClientAsync(cChange, OnlineId);
                                    await SaveChangesAsync(sync.client.ChangeTimeStamp, sync.client.Change, sync.Table, EntryId, sync.client.ChangeUserOnlineEntryId);
                                    break;
                                }
                                save = await _sservice.UploadClientAsync(cChange, cId);
                                mapList.Add(save);
                                await SaveChangesAsync(sync.client.ChangeTimeStamp, sync.client.Change, sync.Table, save.OnlineEntryId, sync.client.ChangeUserOnlineEntryId);
                            }
                            else
                            {
                                int EntryId = await _sservice.UploadOldClientAsync(cChange, sync.client.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.client.ChangeTimeStamp, sync.client.Change, sync.Table, EntryId, sync.client.ChangeUserOnlineEntryId);
                            }
                            break;


                        case "Projects":
                            var prChange = _mapper.Map<Project>(sync.project);
                            int prId = sync.project.Id;
                            if (sync.project.DepartmentOnlineEntryId is null)
                            {
                                foreach (var mapped in mapList)
                                {
                                    if (mapped.Id == sync.project.DepartmentId && mapped.Table == "Departments")
                                    {
                                        prChange.DepartmentId = mapped.OnlineEntryId;
                                        break;
                                    }
                                }
                            }

                            if (sync.project.OnlineEntryId is null)
                            {
                                if (sync.project.Change.CompareTo("EDIT") == 0)
                                {
                                    int OnlineId = 0;
                                    foreach (var map in mapList)
                                    {
                                        if (map.Id == sync.project.Id && map.Table.CompareTo("Projects") == 0)
                                        {
                                            OnlineId = map.OnlineEntryId;
                                            break;
                                        }
                                    }
                                    int EntryId = await _sservice.UploadOldProjectAsync(prChange, OnlineId);
                                    await SaveChangesAsync(sync.project.ChangeTimeStamp, sync.project.Change, sync.Table, EntryId, sync.project.ChangeUserOnlineEntryId);
                                    break;

                                }
                                save = await _sservice.UploadProjectAsync(prChange, prId);
                                mapList.Add(save);
                                await SaveChangesAsync(sync.project.ChangeTimeStamp, sync.project.Change, sync.Table, save.OnlineEntryId, sync.project.ChangeUserOnlineEntryId);
                            }
                            else
                            {
                                int EntryId = await _sservice.UploadOldProjectAsync(prChange, sync.project.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.project.ChangeTimeStamp, sync.project.Change, sync.Table, EntryId, sync.project.ChangeUserOnlineEntryId);
                            }
                            break;

                        case "CostCategories":
                            var ccChange = _mapper.Map<CostCategory>(sync.costCategory);
                            int ccId = sync.costCategory.Id;
                            if (sync.costCategory.OnlineEntryId is null)
                            {
                                if (sync.costCategory.Change.CompareTo("EDIT") == 0)
                                {
                                    int OnlineId = 0;
                                    foreach (var map in mapList)
                                    {
                                        if (map.Id == sync.costCategory.Id && map.Table.CompareTo("CostCategories") == 0)
                                        {
                                            OnlineId = map.OnlineEntryId;
                                            break;
                                        }

                                    }
                                    int EntryId = await _sservice.UploadOldCostAsync(ccChange, OnlineId);
                                    await SaveChangesAsync(sync.costCategory.ChangeTimeStamp, sync.costCategory.Change, sync.Table, EntryId, sync.costCategory.ChangeUserOnlineEntryId);
                                    break;
                                }
                                save = await _sservice.UploadCostAsync(ccChange, ccId);
                                mapList.Add(save);
                                await SaveChangesAsync(sync.costCategory.ChangeTimeStamp, sync.costCategory.Change, sync.Table, save.OnlineEntryId, sync.costCategory.ChangeUserOnlineEntryId);

                            }
                            else
                            {
                                int EntryId = await _sservice.UploadOldCostAsync(ccChange, sync.costCategory.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.costCategory.ChangeTimeStamp, sync.costCategory.Change, sync.Table, EntryId, sync.costCategory.ChangeUserOnlineEntryId);
                            }
                            break;

                        case "Expenditures":
                            var eChange = _mapper.Map<Expenditure>(sync.expenditure);
                            int eId = sync.expenditure.Id;
                            if (sync.expenditure.ClientOnlineEntryId is null)
                            {
                                foreach (var mapped in mapList)
                                {
                                    if (mapped.Id == sync.expenditure.ClientId && mapped.Table.CompareTo("Clients") == 0)
                                    {
                                        eChange.ClientId = mapped.OnlineEntryId;
                                        break;
                                    }
                                }
                            }
                            if (sync.expenditure.CostCategoryOnlineEntryId is null)
                            {
                                foreach (var mapped in mapList)
                                {
                                    if (mapped.Id == sync.expenditure.CostCategoryId && mapped.Table.CompareTo("CostCategories") == 0)
                                    {
                                        eChange.CostCategoryId = mapped.OnlineEntryId;
                                        break;
                                    }
                                }
                            }
                            if (sync.expenditure.ProjectOnlineEntryId is null)
                            {
                                foreach (var mapped in mapList)
                                {
                                    if (mapped.Id == sync.expenditure.ProjectId && mapped.Table.CompareTo("Projects") == 0)
                                    {
                                        eChange.ProjectId = mapped.OnlineEntryId;
                                        break;
                                    }
                                }
                            }
                            if (sync.expenditure.IssuerOnlineEntryId is null)
                            {
                                foreach (var mapped in mapList)
                                {
                                    if (mapped.Id == sync.expenditure.IssuerId && mapped.Table.CompareTo("Users") == 0)
                                    {
                                        eChange.IssuerId = mapped.OnlineEntryId;
                                        break;
                                    }
                                }
                            }
                            if (sync.expenditure.OnlineEntryId is null)
                            {
                                if (sync.expenditure.Change == "EDIT")
                                {
                                    int OnlineId = 0;
                                    foreach (var map in mapList)
                                    {
                                        if (map.Id == sync.expenditure.Id && map.Table.CompareTo("Expenditures") == 0)
                                        {
                                            OnlineId = map.OnlineEntryId;
                                            break;
                                        }
                                    }
                                    int EntryId = await _sservice.UploadOldExpenditureAsync(eChange, OnlineId);
                                    await SaveChangesAsync(sync.expenditure.ChangeTimeStamp, sync.expenditure.Change, sync.Table, EntryId, sync.expenditure.ChangeUserOnlineEntryId);
                                    break;
                                }
                                save = await _sservice.UploadExpenditureAsync(eChange, eId);

                                mapList.Add(save);
                                await SaveChangesAsync(sync.expenditure.ChangeTimeStamp, sync.expenditure.Change, sync.Table, save.OnlineEntryId, sync.expenditure.ChangeUserOnlineEntryId);
                            }
                            else
                            {
                                int EntryId = await _sservice.UploadOldExpenditureAsync(eChange, sync.expenditure.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.expenditure.ChangeTimeStamp, sync.expenditure.Change, sync.Table, EntryId, sync.expenditure.ChangeUserOnlineEntryId);

                            }
                            break;


                        case "Services":
                            var sChange = _mapper.Map<Service>(sync.service);
                            int sId = sync.service.Id;
                            if (sync.service.ProjectOnlineEntryId is null)
                            {
                                foreach (var mapped in mapList)
                                {
                                    if (mapped.Id == sync.service.ProjectId && mapped.Table.CompareTo("Projects") == 0)
                                    {
                                        sChange.ProjectId = mapped.OnlineEntryId;
                                        break;
                                    }
                                }
                            }
                            sChange.FixedAmount = sync.service.FixedAmount == 1 ? true : false;
                            if (sync.service.OnlineEntryId is null)
                            {
                                if (sync.service.Change == "EDIT")
                                {
                                    int OnlineId = 0;
                                    foreach (var map in mapList)
                                    {
                                        if (map.Id == sync.service.Id && map.Table.CompareTo("Services") == 0)
                                        {
                                            OnlineId = map.OnlineEntryId;
                                            break;
                                        }
                                    }
                                    int EntryId = await _sservice.UploadOldServiceAsync(sChange, OnlineId);
                                    await SaveChangesAsync(sync.service.ChangeTimeStamp, sync.service.Change, sync.Table, EntryId, sync.service.ChangeUserOnlineEntryId);
                                    break;
                                }
                                save = await _sservice.UploadServiceAsync(sChange, sId);
                                mapList.Add(save);
                                await SaveChangesAsync(sync.service.ChangeTimeStamp, sync.service.Change, sync.Table, save.OnlineEntryId, sync.service.ChangeUserOnlineEntryId);

                            }
                            else
                            {
                                int EntryId = await _sservice.UploadOldServiceAsync(sChange, sync.service.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.service.ChangeTimeStamp, sync.service.Change, sync.Table, EntryId, sync.service.ChangeUserOnlineEntryId);

                            }
                            break;

                        case "Incomes":
                            var iChange = _mapper.Map<Income>(sync.income);
                            if (sync.income.ClientOnlineEntryId == null)
                            {
                                foreach (var saved in mapList)
                                {
                                    if (saved.Id == sync.income.ClientId && saved.Table.CompareTo("Clients") == 0)
                                    {
                                        iChange.ClientId = saved.OnlineEntryId;
                                        break;
                                    }

                                }
                            }
                            if (sync.income.ServiceOnlineEntryId == null)
                            {
                                foreach (var saved in mapList)
                                {
                                    if (saved.Id == sync.income.ServiceId && saved.Table.CompareTo("Services") == 0)
                                    {
                                        iChange.ServiceId = saved.OnlineEntryId;
                                        break;
                                    }
                                }
                            }

                            if (sync.income.IncomeOnlineEntryId == null && sync.income.IncomeId != 0)
                            {
                                foreach (var saved in mapList)
                                {
                                    if (saved.Id == sync.income.IncomeId && saved.Table.CompareTo("Incomes") == 0)
                                    {
                                        iChange.IncomeId = saved.OnlineEntryId;
                                        break;
                                    }
                                }
                            }

                            if (sync.income.UserOnlineEntryId == null)
                            {
                                foreach (var saved in mapList)
                                {
                                    if (saved.Id == sync.income.UserId && saved.Table.CompareTo("Users") == 0)
                                    {
                                        iChange.UserId = saved.OnlineEntryId;
                                        break;
                                    }
                                }
                            }


                            int iId = sync.income.Id;
                            if (sync.income.OnlineEntryId is null)
                            {
                                if (sync.income.Change == "EDIT")
                                {
                                    int OnlineId = 0;
                                    foreach (var map in mapList)
                                    {

                                        if (map.Id == sync.income.Id)
                                        {
                                            OnlineId = map.OnlineEntryId;
                                            break;
                                        }
                                    }
                                    int EntryId = await _sservice.UploadOldIncomeAsync(iChange, OnlineId);
                                    await SaveChangesAsync(sync.income.ChangeTimeStamp, sync.income.Change, sync.Table, EntryId, sync.income.ChangeUserOnlineEntryId);

                                    break;
                                }
                                save = await _sservice.UploadIncomeAsync(iChange, iId);
                                mapList.Add(save);
                                await SaveChangesAsync(sync.income.ChangeTimeStamp, sync.income.Change, sync.Table, save.OnlineEntryId, sync.income.ChangeUserOnlineEntryId);
                            }
                            else
                            {
                                int EntryId = await _sservice.UploadOldIncomeAsync(iChange, sync.income.OnlineEntryId.Value);
                                await SaveChangesAsync(sync.income.ChangeTimeStamp, sync.income.Change, sync.Table, EntryId, sync.income.ChangeUserOnlineEntryId);

                            }
                            break;

                            /*case "Changes" :
                                int DesktopClientId = int.Parse(this.User.Claims.First(x =>x.Type == "Desktopid").Value);

                                await _context.Changes.AddAsync(sync.change);
                                break;*/
                    }
                }
                return Ok(mapList);
            }
            catch (System.Exception e)
            {
                throw e;
            }

        }



        [Authorize]
        [HttpGet("Download/{lastSyncID}/{isFirst}")]
        public async Task<IActionResult> DownloadData([FromRoute]int lastSyncID, bool isFirst)
        {

            const int numnerOfItems = 10;
            string MacAddress = this.User.Claims.First(i => i.Type == "macAddr").Value;

            var dc = await _context.DesktopClients.Where((x) => x.ClientMacAddress.CompareTo(MacAddress) == 0).FirstOrDefaultAsync();
            List<Change> lastChanges;
            if (!isFirst)
            {
                lastChanges = await _context.Changes
                                    .Where(i => i.Id > lastSyncID && i.DesktopClientId != dc.Id)
                                    //.Except(_context.Changes.Where(((x) => x.DesktopClientId == dc.Id)))
                                    .OrderBy(x => x.Id).Take(numnerOfItems).ToListAsync();
            }
            else
            {
                lastChanges = await _context.Changes
                                    .Where(i => i.Id > lastSyncID)
                                    .OrderBy(x => x.Id).Take(numnerOfItems).ToListAsync();
            }

            List<SyncViewModel> syncValues = new List<SyncViewModel>();

            foreach (var ch in lastChanges)
            {
                SyncViewModel obj = new SyncViewModel();
                switch (ch.Table)
                {
                    case "Departments":
                        obj.dept = await _sservice.DownloadDeptAsync(ch);
                        break;

                    /*case "Persons":
                        obj.person = await _sservice.DownloadPersonAsync(ch);
                        break;*/

                    case "Users":
                        obj.user = await _sservice.DownloadUserAsync(ch);
                        break;
                    case "Clients":
                        obj.client = await _sservice.DownloadClientAsync(ch);
                        break;
                    case "Projects":
                        obj.project = await _sservice.DownloadProjectAsync(ch);
                        break;
                    case "CostCategories":
                        obj.costCategory = await _sservice.DownloadCostAsync(ch);
                        break;
                    case "Expenditures":
                        obj.expenditure = await _sservice.DownloadExpenditureAsync(ch);
                        break;
                    case "Services":
                        obj.service = await _sservice.DownloadServicesAsync(ch);
                        break;
                    case "Incomes":
                        obj.income = await _sservice.DownloadIncomesAsync(ch);
                        break;
                    
                }
                obj.Table = ch.Table;
                syncValues.Add(obj);
            }

            return Ok(syncValues);
        }
    }
}