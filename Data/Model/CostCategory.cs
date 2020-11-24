
using System;
using System.Collections.Generic;

namespace ListaccFinance.API.Data.Model
{
    public class CostCategory
    {
       

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public ICollection<Expenditure> Expenditures { get;set; }
    }
}