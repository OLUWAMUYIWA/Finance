
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Data.ViewModel;
using ListaccFinance.API.SendModel;

namespace ListaccFinance.API.Interfaces
{
    public interface ISyncService
    //<T> where T : class

    {

        //Downloads
        Task<DepartMentViewModel> DownloadDeptAsync(Change ch);
        Task<PersonViewModel> DownloadPersonAsync(Change ch);
        Task<UserViewModel> DownloadUserAsync(Change ch);
        Task<ClientViewModel> DownloadClientAsync(Change ch);
        Task<ProjectViewModel> DownloadProjectAsync(Change ch);
        Task<CostCategoryViewModel> DownloadCostAsync(Change ch);
        Task<ExpenditureViewModel> DownloadExpenditureAsync(Change ch);
        Task<ServiceViewModel> DownloadServicesAsync(Change ch);
        Task<IncomeViewModel> DownloadIncomesAsync(Change ch);


        //Uploads

        bool IsExist<T>(int Id) where T : class;
        Task<SavedList> UploadDeptAsync(Department d, int OffId);
        Task<int> UploadOldDeptAsync(Department d, int OnlineId);
        Task<SavedList> UploadPersonAsync(Person p, int OffId);
        Task<int> UploadOldPersonAsync(Person p, int OnlineId);
        Task<SavedList> UploadClientAsync(Client c, int OldId);
        Task<int> UploadOldClientAsync(Client c, int OnlineId);
        Task<SavedList> UploadProjectAsync(Project p, int OldId);
        Task<int> UploadOldProjectAsync(Project p, int OnlineId);
        Task<SavedList> UploadCostAsync(CostCategory c, int OffId);
        Task<int> UploadOldCostAsync(CostCategory c, int OnlineId);
        Task<SavedList> UploadExpenditureAsync(Expenditure e, int OffId);
        Task<int> UploadOldExpenditureAsync(Expenditure e, int OnlineId);
        Task<SavedList> UploadServiceAsync(Service s, int OldId);
        Task<int> UploadOldServiceAsync(Service s, int OnlineId);
        Task<SavedList> UploadIncomeAsync(Income i, int OldId);
        Task<int> UploadOldIncomeAsync(Income i, int OnlineId);
        Task HandleChangeUpload(Change ch);
    }

}
