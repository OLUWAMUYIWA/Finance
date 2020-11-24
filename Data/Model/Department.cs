
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ListaccFinance.API.Data.Model
{
    public class Department
    {
        public int Id {get;set;}
        [Required]
        public string Name {get;set;}
        public ICollection<Project> Projects {get;set;}
    }
}