using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ListaccFinance.API.Data.Model;

namespace ListaccFinance.API.Services
{
    public class DownloadContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override bool TryComputeLength(out long length)
        {
            throw new System.NotImplementedException();
        }

        public List<Department> deptList {get; set;}
        public List<Person> pertList { get; set; }
        public List<User> userList {get; set;}
        public List<Client> cList{get; set;}

        public List<Project> proList {get; set;}
        public List<CostCategory> costList{get; set;}
        public List<Expenditure> expList {get; set;}
        public List<Service> serList{get; set;}
        public List<Income> incList {get; set;}

        public int lastId {get; set;}
         
    }
}