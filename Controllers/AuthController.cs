using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ListaccFinance.Api.Data;
using ListaccFinance.API.Data.ViewModel;
using ListaccFinance.API.Interfaces;
using ListaccFinance.API.SendModel;
using ListaccFinance.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListaccFinance.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly ITokenGenerator _tokGen;

        private readonly IDesktopService _dService;

        private readonly ISyncService _sservice;
        private readonly IMapper _mapper;
        private readonly IOtherServices _oService;

        public AuthController(DataContext context,
                                ITokenGenerator tokGen,
                                IDesktopService dService,
                                ISyncService sservice,
                                IMapper mapper,
                                IOtherServices oService
                             )
        {
            _context = context;
            _tokGen = tokGen;
            _dService = dService;
            _sservice = sservice;
            _mapper = mapper;
            _oService = oService;

        }

        // This method pings the server at intervals
        [HttpGet("PingServer")]
        public IActionResult PingServer()
        {
            return Ok();
        }

        [HttpPost("DesktopLogin")]
        public async Task<IActionResult> Login(SyncLoginModel mod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Password Hash
            var currentUser = _context.Users.Where(x => x.Email.ToUpper().CompareTo(mod.EmailAddress.ToUpper()) == 0 && x.Status == true).FirstOrDefault();
            if (currentUser == null || !Hash.Validate(mod.Password, currentUser.salt, currentUser.PasswordHash))
            {
                return BadRequest(new { message = "Your login input is incorrect" });
            }

            var d = await _context.DesktopClients.Where(x => mod.ClientName.ToUpper().CompareTo(x.ClientName.ToUpper()) == 0
                                             && mod.ClientMacAddress.ToUpper().CompareTo(x.ClientMacAddress.ToUpper()) == 0
                                             && mod.ClientType.ToUpper().CompareTo(x.ClientType.ToUpper()) == 0).FirstOrDefaultAsync();

            //gets the type of the user
            var type = _oService.Strip(currentUser.GetType().ToString());

            if (d == null)
            {
                var dc = new DesktopCreateModel()
                {
                    ClientMacAddress = mod.ClientMacAddress,
                    ClientName = mod.ClientName,
                    ClientType = mod.ClientType,
                };
                d = await _dService.CreateDesktopClientAsync(dc);
            }

            var token = await _tokGen.GenerateToken(d, currentUser.Id, type);


            return Ok(token);

        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> Login(UserLogin u)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Password Hash Comparison
            var pmessage = u.Password;
            var currentUser = _context.Users.Where(x => x.Email.ToUpper().CompareTo(u.EmailAddress.ToUpper()) == 0 && x.Status == true).Include(x => x.Person).FirstOrDefault();

            if (currentUser is null)
            {
                return BadRequest(new { message = "Not authorized" });
            }
            if (currentUser.GetType().Name.ToLower().CompareTo("admin") != 0)
            {
                return Unauthorized(new {message = "You're not an admin member. This is above your paygrade"});
            }
            else
            {
                var PasswordHash = Hash.Create(pmessage, currentUser.salt);
                //var isCorrect = Hash.Validate(pmessage, salt, PasswordHash);

                if (currentUser.PasswordHash.CompareTo(PasswordHash) == 0)
                {
                    int myID = currentUser.Id;

                    //gets the type of the user
                    var type = _oService.Strip(currentUser.GetType().ToString());

                    var tokenString = await _tokGen.GenerateToken(u, myID, type);
                    var theCurrentUser = _mapper.Map<CurrentUser>(currentUser);
                    
                    var token = new
                    {
                        tokenString = tokenString,
                        currentUser = theCurrentUser
                    };
                    return Ok(token);
                }
                return BadRequest("Wrong password");
            }


            /*var thisUser = _context.Users.
                Where(x => x.Email.CompareTo(u.EmailAddress) == 0 &&
                 x.PasswordHash.CompareTo(PasswordHash) == 0).FirstOrDefault();*/


        }

    }
}