
using System;
using ListaccFinance.API.Data.Model;

namespace ListaccFinance.API.Data.ViewModel
{
    public class DeptView
    {
        public int Id {get; set;}
        public string Name {get; set;}
    }
    
    // Return for search
    public class SearchProps
    {
        //Search Terms
        public int Id {get; set;}
        public string LastName {get; set;}
        public string FirstName {get; set;}
        public string Gender {get; set;}
        public string Email {get; set;}
        public string Phone { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }

    }
    
    public class SearchDept
    {
        public string SearchString {get; set;}
        public int PageNumber { get; set; } = 1;
        public int _pageSize { get; set; } = 5;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        const int MaxPageSize = 20;
    }
    
    public class SearchPaging
    {
        public string SearchString { get; set; }

        //Filter
        public string[] Role { get; set; }
        public bool Status { get; set; }
        // Pagination
        public int PageNumber { get; set; } = 1;
        public int _pageSize { get; set; } = 5;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        const int MaxPageSize = 20;
    }

    public class CurrentUser
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public Department Department { get; set; }
        public Person Person { get; set; }
        public bool Status { get; set; }
    }
    
    public class SyncViewModel
    {
        public DepartMentViewModel dept {get; set;}
        public PersonViewModel person{get; set;} 
        public UserViewModel user {get; set;} 
        public ClientViewModel client {get; set;}
        public ProjectViewModel project {get; set;}
        public CostCategoryViewModel costCategory{get; set;}
        public ExpenditureViewModel expenditure {get; set;}
        public ServiceViewModel service{get; set;}
        public IncomeViewModel income {get; set;}
        public Change change {get; set;}
        public string Table {get; set;} 

    }

    public class DepartMentViewModel 
    {
        public DepartMentViewModel()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int OnlineEntryId {get; set;}
        public int ChangeId {get; set;}
    }

    public class PersonViewModel
    {
        public PersonViewModel()
        {}
        public int Id { get; set; }
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }
    
    public class UserViewModel : IdentityU
    {
        public UserViewModel ()
        {}
        public int Id {get; set;}
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public Person person {get; set;}
        public int PersonId { get; set; }
        public string salt { get; set; }
        public bool Status {get; set;}

        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
        public string Discriminator {get; set;}
       // public string Email {get; set;}
    }

    public class IdentityU {
        public string Email {get; set;}
        public string PasswordHash{get; set;}
    }

    public class ClientViewModel
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UId { get; set; }
        public string UId2 { get; set; }
        public double AmountReceivable { get; set; }
        public Person Person {get; set;}
        public int? PersonId { get; set; }

        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class ProjectViewModel 
    {
        public ProjectViewModel()
        {}
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int DepartmentId { get; set; }

        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class CostCategoryViewModel
    {
        public CostCategoryViewModel()
        {}
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }

    }

    public class ExpenditureViewModel 
    {
        public int Id {get; set;}
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int CostCategoryId { get; set; }
        public int ProjectId { get; set; }
        public int IssuerId { get; set; }
        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int ProjectId { get; set; }
        public bool FixedAmount { get; set; }
        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }
    }

    public class IncomeViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int ServiceId { get; set; }
        public int ClientId { get; set; }
        public double AmountReceived { get; set; }
        public double Discount { get; set; }
        public string PaymentType { get; set; }
        public double AmountReceivable { get; set; }
        public DateTime DateDue { get; set; }
        public int Unit { get; set; }
        public int IncomeId { get; set; }
        public int UserId { get; set; }
        public int OnlineEntryId { get; set; }
        public int ChangeId { get; set; }

    }
}