using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineChatServer.Application.Users.Commands.RegisterUser;
using OnlineChatServer.Application.Users.Queries.Login;
using OnlineChatServer.DataAccess;
using OnlineChatServer.Domain;
using OnlineChatServer.Domain.Models;
using OnlineChatServer.Models;

namespace OnlineChatServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationSettings _appSettings;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings,
            RoleManager<IdentityRole> roleManager, IMediator mediator, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _appSettings = appSettings.Value;
            _roleManager = roleManager;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserInfo/{userID}")]
        public async Task<object> GetUserInfo(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            var roles = await _userManager.GetRolesAsync(user);
            return new
            {
                UserID = user.Id,
                user.FirstName,
                user.LastName,
                FullName = $"{user.FirstName} {user.LastName}",
                user.Email,
                Login = user.UserName,
                user.ImagePath,
                DateRegister = user.RegisterDate,
                Roles = roles
            };
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(ApplicationUserModel model)
        {
            var result = await _mediator.Send(new RegisterUserCommand
            {
                User = model
            });

            return Ok(result);
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var token = await _mediator.Send(new LoginQuery
            {
                Model = model,
                JWTSecretKey = _appSettings.SecretJWTKey
            });

            if (string.IsNullOrWhiteSpace(token)) return BadRequest("Error Login");

            return Ok(new { token });
        }

        [HttpGet]
        [Authorize]
        [Route("GetAllUsersInfo")]
        public List<ChatUser> GetAllUser()
        {
            var currentUserID = User.Claims.First(x => x.Type == "UserID").Value;
            var result = _db.Users.Select(x => new ChatUser
            {
                UserID = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                ImagePath = x.ImagePath,
                HaveUnreadDialog = false
            }).ToList();

            var currentUser = result.FirstOrDefault(x => x.UserID == currentUserID);
            result.Remove(currentUser);

            foreach (var user in result)
            {
                var unreadDialog =
                    _db.UnreadDialogs.FirstOrDefault(x => x.SenderID == user.UserID && x.UserID == currentUserID);

                if (unreadDialog != null) user.HaveUnreadDialog = true;
            }

            return result;
        }
    }
}