using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ListaccFinance.API.Data.Model
{
    public class Project
    {
        public int Id {get;set;}
        [Required]
        public string Name {get;set;}
        public string Description {get;set;}
        public int DepartmentId {get;set;}
        public Department Department {get;set;}
        public ICollection<Service> Services {get;set;}
        public ICollection<Expenditure> Expenditures {get;set;}
    }
}