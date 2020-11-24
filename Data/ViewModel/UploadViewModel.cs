using System;
using ListaccFinance.API.Data.Model;

namespace ListaccFinance.API.Data.ViewModel
{
    public class SavedList
    {
        public int Id {get; set;}
        public string Table {get; set;}
        public int OnlineEntryId { get; set; }
    }
    public class UploadSyncViewModel
    {
        public DepartmentUpViewModel dept { get; set; }
        /*public PersonUpViewModel person { get; set; }
        public UserUpViewModel user { get; set; }*/
        public ClientUpViewModel client { get; set; }
        public ProjectUpViewModel project { get; set; }
        public CostCategoryUpViewModel costCategory { get; set; }
        public ExpenditureUpViewModel expenditure { get; set; }
        public ServiceUpViewModel service { get; set; }
        public IncomeUpViewModel income { get; set; }
        public string Table { get; set; }
    }

    public class UpViewModel 
    {
        public int ChangeUserOnlineEntryId {get; set;}
        public string Change {get; set;}
        public DateTime ChangeTimeStamp  {get; set;}
    }

    public class DepartmentUpViewModel : UpViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class PersonUpViewModel :UpViewModel
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class UserUpViewModel : UpViewModel
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public int? DepartmentOnlineEntryId {get; set;}
        public Person person {get; set;}
        public int PersonId { get; set; }
        public int PersonOnlineEntryId {get; set;}
        public string salt { get; set; }
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }

        public string Email{get; set;}
    }

    public class ClientUpViewModel: UpViewModel
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UId { get; set; }
        public string UId2 { get; set; }
        public double AmountReceivable { get; set; }
        public Person Person { get; set; }
        public int? PersonId { get; set; }
        public int? PersonOnlineEntryId {get; set;}

        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class ProjectUpViewModel : UpViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int? DepartmentOnlineEntryId {get; set;}
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class CostCategoryUpViewModel: UpViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class ExpenditureUpViewModel: UpViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int? ClientOnlineEntryId {get; set;}
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int CostCategoryId { get; set; }
        public int? CostCategoryOnlineEntryId {get; set;}
        public int ProjectId { get; set; }
        public int? ProjectOnlineEntryId {get; set;}
        public int IssuerId { get; set; }
        public int? IssuerOnlineEntryId{get; set;}
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class ServiceUpViewModel : UpViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int FixedAmount { get; set; }
        public int ProjectId { get; set; }
        public int? ProjectOnlineEntryId {get; set;}
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class IncomeUpViewModel : UpViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int ServiceId { get; set; }
        public int? ServiceOnlineEntryId {get; set;}
        public int ClientId { get; set; }
        public int? ClientOnlineEntryId{get; set;}
        public double AmountReceived { get; set; }
        public double Discount { get; set; }
        public string PaymentType { get; set; }
        public double AmountReceivable { get; set; }
        public DateTime DateDue { get; set; }
        public int Unit { get; set; }
        public int IncomeId { get; set; }
        public int? IncomeOnlineEntryId {get; set;}
        public int UserId { get; set; }
        public int? UserOnlineEntryId {get; set;}
        public int? OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }
}