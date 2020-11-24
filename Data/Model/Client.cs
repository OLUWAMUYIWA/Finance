using System.Collections.Generic;

namespace ListaccFinance.API.Data.Model
{
    public class Client
    {
        public int Id {get;set;}
        public string BusinessName {get;set;}
        public string Phone {get;set;}
        public string Email {get;set;}
        public string Address {get;set;}
        public string UId {get;set;}
        public string UId2 {get;set;}
        public double AmountReceivable {get;set;}
        public Person Person {get;set;}
        public int? PersonId { get; set; }
        public ICollection<Expenditure> Expenditures { get; set; }
        
    }
}