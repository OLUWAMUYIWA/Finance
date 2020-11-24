using System;
using System.Linq;
using System.Threading.Tasks;
using ListaccFinance.Api.Data;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Data.ViewModel;
using ListaccFinance.API.Interfaces;
using ListaccFinance.API.SendModel;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace ListaccFinance.API.Services

{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IOtherServices _oService;
        private readonly IMapper _mapper;
        private readonly IUserRepo _urepo;


        public UserService(DataContext context, IOtherServices oService, IMapper mapper, IUserRepo uRepo)
        {
            _oService = oService;
            _context = context;
            _mapper = mapper;
            _urepo = uRepo;
        }


        //First User Creation
        public async Task<string> CreateUserAsync(RegisterModel reg)
        {
            var newUser = new Admin();

            var dept = new Department()
            {
                Name = reg.Department
            };

            var per = new Person()
            {
                firstName = reg.firstName,
                LastName = reg.LastName,
                Gender = reg.Gender,
                DateOfBirth = reg.DateOfBirth,
            };

            newUser.Email = reg.EmailAddress;
            newUser.Address = reg.Address;
            newUser.Phone = reg.Phone;
            newUser.Status = true;

            // Password Hash
            // var message = reg.Password;
            var message = reg.LastName.ToLower();
            var salt = Salt.Create();
            var hash = Hash.Create(message, salt);
            newUser.PasswordHash = hash;
            newUser.salt = salt;

            newUser.Person = per;
            newUser.Department = dept;

            newUser.SearchString = (newUser.Person.LastName + " " + newUser.Person.firstName + " " + newUser.Person.Gender + " " + newUser.Email + " " + newUser.Phone + " Administrator" + " " + "Active").ToUpper();      
            
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();


            // retrieve id of newly created item
            var thisUser = _context.Admins.
                Where(x => x.Email.CompareTo(reg.EmailAddress) == 0 &&
                 x.PasswordHash.CompareTo(hash) == 0).FirstOrDefault();

            int thisUserID = thisUser.Id;

            var change = new Change()
            {
                Table = "Users",
                ChangeType = "Create",
                EntryId = thisUserID,
                OnlineTimeStamp = DateTime.Now,
                OfflineTimeStamp = DateTime.Now,
            };
            var changeDept = new Change() 
            {
                Table = "Departments",
                ChangeType = "Create",
                EntryId = thisUserID,
                OnlineTimeStamp = DateTime.Now,
                OfflineTimeStamp = DateTime.Now
            };

            await _context.Changes.AddAsync(changeDept);
            await _context.SaveChangesAsync();
            await _context.Changes.AddAsync(change);
            await _context.SaveChangesAsync();

            return "done";


        }

        //Subsequent User(member) Creation
        public async Task<string> CreateUserAsync(RegisterModel reg, int userId)
        {

            var newUser = new Member();

            // var dept = new Department()
            // {
            //     Name = reg.Department
            // };

            var per = new Person()
            {
                
                firstName = reg.firstName,
                LastName = reg.LastName,
                Gender = reg.Gender,
                DateOfBirth = reg.DateOfBirth,
            };

            newUser.Email = reg.EmailAddress;
            newUser.Address = reg.Address;
            newUser.Phone = reg.Phone;
            newUser.Status = true;

            // var DeptCheck = _context.Departments.Where(x => x.Id == reg.DepartmentId).FirstOrDefaultAsync();
            // string errMessage = "Department does not exist";
            // if (DeptCheck == null)
            // {
            //     throw new Exception(errMessage);
            // }
            newUser.DepartmentId = reg.DepartmentId.Value;



            // Password Hash
            //var message = reg.Password;
            var message = reg.LastName.ToLower();
            var salt = Salt.Create();
            var hash = Hash.Create(message, salt);
            newUser.PasswordHash = hash;
            newUser.salt = salt;


            newUser.Person = per;
            //newUser.Department = dept;


            //await _context.SaveChangesAsync();
            newUser.SearchString = (newUser.Person.LastName + " " + newUser.Person.firstName + " " + newUser.Person.Gender + " " + newUser.Email + " " + newUser.Phone + " " + newUser.Status + " Member"  +" " + "Active").ToUpper();
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();


            var thisUser = _context.Members.
                            Where(x => x.Email.CompareTo(reg.EmailAddress) == 0 &&
                             x.PasswordHash.CompareTo(hash) == 0).FirstOrDefault();

            int thisUserID = thisUser.Id;

            var change = new Change()
            {
                Table = "Users",
                EntryId = thisUserID,
                ChangeType = "Create",
                OnlineTimeStamp = DateTime.Now,
                OfflineTimeStamp = DateTime.Now,
                UserId = userId,
            };
            await _context.Changes.AddAsync(change); ;
            await _context.SaveChangesAsync();


            return "done";


        }



    

        public bool IsUserExist()
        {
            var nr = _context.Users.Count();
            if (nr == 0)
            {
                return false;
            }

            return true;

            /*
            public bool IsUserExist (UserLogin u)
            {
            var i = _context.Users.Where(x => x.UserName.CompareTo(u.UserName) == 0 && x.Password.CompareTo(u.Password) == 0).FirstOrDefault();
            if (i is null)
            {
                return false;
            }
            }

            return true; */
        }

        public async Task EditUserAsync(int Id, RegisterModel reg, int MyId)
        {
            var u = await _context.Users.Include(x => x.Person).Where(x => x.Id == Id).FirstOrDefaultAsync();

            // general info
            u.Person.LastName = reg.LastName;
            u.Person.firstName = reg.firstName;
            u.Person.Gender = reg.Gender;
            u.Person.DateOfBirth = reg.DateOfBirth;

            // contact info
            u.Email = reg.EmailAddress;
            u.Phone = reg.Phone;
            
            // department
            if (reg.DepartmentId != null)
            {
                u.DepartmentId = reg.DepartmentId.Value;
            }
            else if (!String.IsNullOrWhiteSpace(reg.Department)){
                u.DepartmentId = _context.Departments.Where(x => x.Name.CompareTo(reg.Department) ==0).FirstOrDefaultAsync().Id;
            }

            // status
            switch (reg.Status.ToLower())
            {
                case "true":
                    u.Status = true;
                    break;
                case "false":
                    u.Status = false;
                    break;       
                default:
                    break;
            }
            //u.Status = reg.Status == "true"? true:false;

            // address
            u.Address = reg.Address;

            // search string
            string newStatus = u.Status == true? "Active" : "Inactive";
            bool isAdmin = u is Admin;
            u.SearchString = String.Format(reg.LastName + " " + reg.firstName + " " + reg.Gender + " " + reg.EmailAddress + " " + reg.Phone + (isAdmin ? " Administrator" : " Member") + " " +  newStatus ).ToUpper();

            // Do password Change later

            // await _context.SaveChangesAsync();
            var change = new Change
            {
                Table = "Users",
                EntryId = u.Id,
                ChangeType = "Edit",
                OfflineTimeStamp = DateTime.Now,
                OnlineTimeStamp = DateTime.Now,
                UserId = MyId
            };
            await _context.Changes.AddAsync(change);

            // save all changes
            await _context.SaveChangesAsync();

        }
        public async Task<bool> IsThisUserExist(string UserEmail)
        {
            var thisU = await _context.Users.Where(x => x.Email.ToUpper().CompareTo(UserEmail.ToUpper()) == 0).FirstOrDefaultAsync();

            if (thisU is null)
            {
                return false;
            }

            return true;
        }
        public async Task CreateAdmin(RegisterModel reg, int userId)
        {
            var newUser = new Admin();

    
            var per = new Person()
            {
                firstName = reg.firstName,
                LastName = reg.LastName,
                Gender = reg.Gender,
                DateOfBirth = reg.DateOfBirth,
            };

            newUser.Email = reg.EmailAddress;
            newUser.Address = reg.Address;
            newUser.Phone = reg.Phone;


            // Password Hash
            //var message = reg.Password;
            var message = reg.LastName.ToLower();
            var salt = Salt.Create();
            var hash = Hash.Create(message, salt);
            newUser.PasswordHash = hash;
            newUser.salt = salt;
            newUser.Status = true;


            newUser.Person = per;

            // var DeptCheck = await _context.Departments.Where(x => x.Id == reg.DepartmentId).FirstOrDefaultAsync();
            // string errMessage = "Department does not exist";
            // if (DeptCheck == null)
            // {
            //     throw new Exception(errMessage);
                
            // }
            newUser.DepartmentId = reg.DepartmentId.Value;//_context.Departments.SingleOrDefaultAsync(x => x.Name.ToUpper().CompareTo(reg.Department.ToUpper()) == 0).Id;

           
            //await _context.SaveChangesAsync();
            newUser.SearchString = (newUser.Person.LastName + " " + newUser.Person.firstName + " " + newUser.Person.Gender + " " + newUser.Email + " " + newUser.Phone + " " + newUser.Status + " Administrator" + " " + "Active").ToUpper();
            await _context.Admins.AddAsync(newUser);
            await _context.SaveChangesAsync();
          
            var thisUser = _context.Admins.
                            Where(x => x.Email.CompareTo(reg.EmailAddress) == 0 &&
                             x.PasswordHash.CompareTo(hash) == 0).FirstOrDefault();

            int thisUserID = thisUser.Id;

            var change = new Change()
            {
                Table = "Users",
                EntryId = thisUserID,
                ChangeType = "Create",
                OnlineTimeStamp = DateTime.Now,
                OfflineTimeStamp = DateTime.Now,
                UserId = userId,
            };
            await _context.Changes.AddAsync(change); ;
            await _context.SaveChangesAsync();

        }

        public async Task Deactivate(int Id, int MyId)
        {
            User u = await _context.Users.Include(x => x.Person).SingleOrDefaultAsync(x => x.Id == Id);
            u.Status = false;
            u.SearchString = (u.Person.LastName + " " + u.Person.firstName + " " + u.Person.Gender + " " + u.Email + " " + u.Phone + " " + u.Status + " Administrator" + " " + "Inactive").ToUpper();
            await _context.SaveChangesAsync();
            var change = new Change
            {
                Table = u.GetType().Name,
                EntryId = u.Id,
                ChangeType = "Edit",
                OfflineTimeStamp = DateTime.Now,
                OnlineTimeStamp = DateTime.Now,
                UserId = MyId
            };
            await _context.Changes.AddAsync(change);
            await _context.SaveChangesAsync();
        }

        
        public async Task Activate(int Id, int MyId)
        {
            User u = await _context.Users.Include(x => x.Person).SingleOrDefaultAsync(x => x.Id == Id);
            u.Status = true;
            u.SearchString = (u.Person.LastName + " " + u.Person.firstName + " " + u.Person.Gender + " " + u.Email + " " + u.Phone + " " + u.Status + " Administrator" + " " + "Active").ToUpper();

            await _context.SaveChangesAsync();
            var change = new Change{
                Table = u.GetType().Name,
                EntryId = u.Id,
                ChangeType = "Edit",
                OfflineTimeStamp = DateTime.Now,
                OnlineTimeStamp =DateTime.Now,
                UserId = MyId
            };
            await _context.Changes.AddAsync(change);
            await _context.SaveChangesAsync();
        }

        // Search when Search String is not null
        public async Task<PagedList<User>> ReturnUsers(SearchPaging props)
        {
            var returned = (await SearchUser(props)).OrderBy(x => x.Id).ToList();
            var retList = PagedList<User>.ToPagedList(returned, props.PageNumber, props.PageSize);
            return retList;
        }

        private async Task<IQueryable<User>> SearchUser(SearchPaging props)
        {
            IQueryable<User> users =  Enumerable.Empty<User>().AsQueryable();
            
            
            for (int i = 0; i < props.Role.Length; i++)
            {
                switch (props.Role[i])
                {
                    case "Admin":
                        var a = await _context.Admins.Include(x => x.Person).Where(x =>
                                                    x.Status == props.Status
                                                    &&
                                                    (x.SearchString.Contains(props.SearchString.ToUpper()))).ToListAsync();
                        users = users.Concat(a);
                        break;

                    case "Member":
                        var m = await _context.Members.Include(x => x.Person).Where(x =>
                                            x.Status == props.Status
                                            &&
                                            (x.SearchString.Contains(props.SearchString.ToUpper()))).ToListAsync();
                        users = users.Concat(m);
                        break;

                    default:
                        var u = await _context.Users.Include(x => x.Person).Where(x =>
                                    x.Status == props.Status
                                    &&
                                    (x.SearchString.Contains(props.SearchString.ToUpper()))).ToListAsync();
                        users =users.Concat(u);
                        break;
                }
           }
           return users;

           
        }

        // Search when search string is null
        public async Task<PagedList<User>> ReturnAllUsers(SearchPaging props)
        {
            IQueryable<User> usersQ =  Enumerable.Empty<User>().AsQueryable();
            for (int i = 0; i < props.Role.Length; i++)
            {
                switch (props.Role[i])
                {
                    case "Admin":
                        var admins = await _context.Admins.Include(x => x.Person).Where(
                                        (x) =>
                                        x.Status.CompareTo(props.Status) == 0)
                                        .OrderBy(x => x.Id).ToListAsync();
                        usersQ =usersQ.Concat(admins);
                        break;
                    
                    case "Member":
                        var members = await _context.Members.Include(x => x.Person).Where(
                                        (x) =>
                                        x.Status.CompareTo(props.Status) == 0)
                                        .OrderBy(x => x.Id).ToListAsync();
                        usersQ = usersQ.Concat(members);
                        break;
                    default:
                        var users = await _context.Users.Include(x => x.Person).Where(
                                        (x) =>
                                        x.Status.CompareTo(props.Status) == 0)
                                        .OrderBy(x => x.Id).ToListAsync();
                        usersQ = usersQ.Concat(users);
                        break;
                }
            }

            var returned =  usersQ.ToList();
            var retList = PagedList<User>.ToPagedList(returned, props.PageNumber, props.PageSize);
            return retList;
        }

        public async Task<RegisterModel> ReturnUser(int Id)
        {
            User u = await _urepo.GertUserById(Id);
            var User = _mapper.Map<RegisterModel>(u);
            User.Role = u.GetType().Name;
            User.Status =  _context.Users.FirstOrDefault(x => x.Id == Id).Status? "Active": "Inactive";
            return User;
    }

}
}