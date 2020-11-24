using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace ListaccFinance.API.Data.Model
{
    public class Person
    {
        public int Id {get;set;}
        public string firstName {get;set;}
        public string LastName {get;set;}
        public string Gender {get;set;}
        //public ICollection<Client> Clients { get; set; }
        public DateTime? DateOfBirth { get; set; }


    }
}