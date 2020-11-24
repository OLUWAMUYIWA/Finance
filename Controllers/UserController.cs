using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ListaccFinance.Api.Data;
using ListaccFinance.API.Data.Model;
using ListaccFinance.API.Data.ViewModel;
using ListaccFinance.API.Interfaces;
using ListaccFinance.API.SendModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ListaccFinance.API.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {

        private readonly IUserService _uService;
        private readonly DataContext _context;
        private readonly ITokenGenerator _generator;
        private readonly IMapper _mapper;
        private readonly IOtherServices _oService;

        public UserController(IUserService uservice, DataContext context, ITokenGenerator generator, IMapper mapper, IOtherServices oService )
        {
            _uService =uservice;
            _context = context;
            _generator = generator;
            _mapper = mapper;
            _oService = oService;
        }





        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPost("FirstCreateUser")]
        public async Task<IActionResult> FirstCreateUser(RegisterModel me)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_uService.IsUserExist())
            {
                var resp = await _uService.CreateUserAsync(me);
                return Ok("successful");
            }


            return BadRequest(new {message = "Could not create"});
            //Redirect("http://localhost:5000/api/user/createuser");

        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin(RegisterModel reg)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var DeptCheck = await _context.Departments.Where(x => x.Id == reg.DepartmentId).FirstOrDefaultAsync();
            //string errors = $" ";
           

            if (DeptCheck == null)
            {
                var error = new
                {
                    errors = new { departmentId = "[department selected does not exist]",}, };

                string result = JsonSerializer.Serialize(error);
                return BadRequest(result);
            }

            if (!await _uService.IsThisUserExist(reg.EmailAddress))
            {
                string userIdString = this.User.Claims.First(i => i.Type == "UserID").Value;
                int userId = int.Parse(userIdString);
                var u = new Admin();
                await _uService.CreateAdmin(reg, userId);
                
                return Ok();
            }

            return BadRequest(new { EmailAddress = " User already exists" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("EditUser/{Id}")]
        public async Task<IActionResult> EditUser(RegisterModel reg, int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var DeptCheck = await _context.Departments.Where(x => x.Id == reg.DepartmentId).FirstOrDefaultAsync();
            if (DeptCheck == null)
            {
                var error = new
                {
                    errors = new { departmentId = "[department selected does not exist]", },
                };

                string result = JsonSerializer.Serialize(error);
                return BadRequest(result);
            }


            await _uService.EditUserAsync(Id, reg, int.Parse(this.User.Claims.First(i => i.Type == "UserID").Value));

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateMember")]

        public async Task<IActionResult> CreateMember(RegisterModel me) 
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var DeptCheck = await _context.Departments.Where(x => x.Id == me.DepartmentId).FirstOrDefaultAsync();
            if (DeptCheck == null)
            {
                var error = new
                {
                    errors = new { departmentId = "[department selected does not exist]", },
                };

                string result = JsonSerializer.Serialize(error);
                return BadRequest(result);
            }



            if (!await _uService.IsThisUserExist(me.EmailAddress))
            {
                string userIdString = this.User.Claims.First(i => i.Type == "UserID").Value;
                int userId = int.Parse(userIdString);
                var u = new User();
                var resp = await _uService.CreateUserAsync(me, userId);
                return Ok();
            }

            return BadRequest(new {EmailAddress = " User already exists"});

            //return RedirectToAction(string actionName, string controllerName, object routeValues);

        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost("DeactivateUser")]
        public async Task<IActionResult> Deactivate ([FromQuery]int Id)
        {
            int MyId = int.Parse(this.User.Claims.First(i => i.Type == "UserID").Value);
            await _uService.Deactivate(Id, MyId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ActivateUser")]
        public async Task<IActionResult> Activate([FromQuery]int Id)
        {
            int MyId = int.Parse(this.User.Claims.First(x => x.Type == "UserID").Value);
            await _uService.Activate(Id, MyId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ReturnUsers ([FromQuery] SearchPaging props)
        {
            //List<SearchProps> finalReturn = new List<SearchProps>();
            
            if (!string.IsNullOrEmpty(props.SearchString))
            {
                var userList = await _uService.ReturnUsers(props);
                var returnList = _mapper.Map<List<SearchProps>>(userList);
                
                for (int i =0; i < returnList.Count; i++)
                {
                        returnList.ElementAt(i).Role = userList.ElementAt(i).GetType().Name;

                }

                // for (int j = 0; j < props.Role.Length; j++)
                // {
                //     for (int i = 0; i < returnList.Count; i++)
                //     {
                //         if (returnList.ElementAt(i).Role.CompareTo(props.Role[j]) == 0)
                //         {
                //             finalReturn.Add(returnList.ElementAt(i));
                //         }
                //     }

                // }
                var data =  new{
                    userList.TotalCount,
                    userList.PageSize,
                    userList.CurrentPage,
                    userList.TotalPages,
                    userList.HasNext,
                    userList.HasPrevious
                };
                var users = new {
                    returnList,
                    data,
                };
                return Ok(users);
            }
            else
            {
                var userList = await  _uService.ReturnAllUsers(props);
                var returnList = _mapper.Map<List<SearchProps>>(userList);

                for (int i = 0; i < returnList.Count; i++)
                {
                        returnList.ElementAt(i).Role = userList.ElementAt(i).GetType().Name.ToString();
    
                }

                // for (int j = 0; j < props.Role.Length; j++)
                // {
                //     //
                //     for (int i = 0; i < returnList.Count; i++)
                //     {
                //         if (returnList.ElementAt(i).Role.CompareTo(props.Role[j]) == 0)
                //         {
                //             finalReturn.Add(returnList.ElementAt(i));
                //         }
                //     }
                // }
                var data = new
                {
                    userList.TotalCount,
                    userList.PageSize,
                    userList.CurrentPage,
                    userList.TotalPages,
                    userList.HasNext,
                    userList.HasPrevious
                };
                var users = new
                {
                    returnList,
                    data,
                };
                return Ok(users);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{Id}")]
        public async Task<IActionResult> ReturnUser(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad model");
            }
            var u = await _uService.ReturnUser(Id);
            u.Department = _oService.Strip(u.Department);
            return Ok(u);
        }
    
        
    }
}