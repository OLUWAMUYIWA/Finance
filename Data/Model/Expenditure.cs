
using System;

namespace ListaccFinance.API.Data.Model
{
    public class Expenditure
    {
        public int Id {get;set;}
        public Client Client {get;set;}
        public int ClientId { get; set; }
        public DateTime Date {get;set;}
        public string Description {get;set;}
        public double Amount {get;set;}
        public int CostCategoryId {get;set;}
        public CostCategory CostCategory {get;set;}
        public Project Project { get; set; }
        public int ProjectId {get;set;}       
        public int IssuerId {get;set;}
        public User Issuer {get;set;}

    }
}