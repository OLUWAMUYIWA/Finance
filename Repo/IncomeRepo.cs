using System.Collections.Generic;
using ListaccFinance.Api.Data;
using System.Linq;
using ListaccFinance.API.Data.Model;

namespace ListaccFinance.Api.Repo

{
    public class IncomeRepo
    {
        private readonly DataContext _context ;

        public IncomeRepo(DataContext context) => _context = context;


        public  List<Income> IncomesFromDate(System.DateTime date) =>  _context.Incomes.Where((i)=> i.Date.CompareTo(date) <= 0).ToList();

        public Income GetIncomeById(int Id) => _context.Incomes.Where((x)=>x.Id == Id).FirstOrDefault();



        public string search(string key)
        {
            switch (key)
            {
                case "money":
                    return "money";
                
                case "love":
                    return "I'm a lover of money. Money over women";
   
                default:
                    return "nothing";
            }
        }

    }
}