using AutoMapper;
using caseman.user.api.Dtos;
using caseman.user.api.Errors;
using caseman.user.api.Extensions;
using caseman.user.api.Helpers;
using core.Entities.Identity;
using core.Interfaces;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace caseman.user.api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment env;
        private readonly IEmailService emailService;
        private readonly IGenericRepository<AppUser> _userRepo;

        public UserController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper,
            IWebHostEnvironment _env,
            IEmailService _emailService,
            IGenericRepository<AppUser> _userRepo)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            env = _env;
            emailService = _emailService;
            this._userRepo = _userRepo;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await userManager.FindByEmailFromClaimsPrincipal(User);

            return new UserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        [Authorize]
        [HttpGet("GetUser")]
        public async Task<ActionResult<UserDto>> GetUser([FromQuery] string Id)
        {
            var user = await userManager.FindByIdAsync(Id);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsUserActive = user.IsUserActive.ToString(),
                Role = user.Role,
                FirstTimeLogin = user.FirstTimeLogin.ToString()
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = tokenService.CreateToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsUserActive = user.IsUserActive.ToString(),
                Role = user.Role,
                FirstTimeLogin = user.FirstTimeLogin.ToString()
            };
        }

        [Authorize]
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpPost("createuser")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            if (CheckEmailExistsAsync(createUserDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                { Errors = new[] { "Email address is in use" } });
            }

            var user = new AppUser
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Institution = createUserDto.Institution,
                Role = createUserDto.Role,
                Email = createUserDto.Email,
                UserName = createUserDto.Email
            };

            var result = await userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var subject = $"New User Notification";

            var body = System.IO.File.ReadAllText(Path.Combine(env.ContentRootPath, "Content", "MailTemplate", "NewUserEmail.txt"));
            body = body.Replace("#UserFullName", createUserDto.FirstName);
            body = body.Replace("#Username", createUserDto.Email);
            body = body.Replace("#Password", createUserDto.Password);
            body = body.Replace("#Role", createUserDto.Role);

            emailService.SendEmailAsync(createUserDto.Email, subject, body);

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        [HttpPost("forgotpassword")]
        public async Task<ActionResult<ApiResponse>> GeneratePasswordResetToken([FromQuery] string emailAddress)
        {
            if (!CheckEmailExistsAsync(emailAddress).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                { Errors = new[] { "Email address does not exist" } });
            }

            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == emailAddress.ToLower());

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var subject = $"Password Reset Token";

            var body = System.IO.File.ReadAllText(Path.Combine(env.ContentRootPath, "Content", "MailTemplate", "PasswordResetTokenEmail.txt"));
            body = body.Replace("#UserFullName", user.FirstName);
            body = body.Replace("#Username", user.Email);
            body = body.Replace("#Token", token);

            emailService.SendEmailAsync(user.Email, subject, body);

            return new ApiResponse(200, "Password Reset Token has been sent to your email address");

        }

        [HttpPut("changepassword")]
        public async Task<ActionResult<ApiResponse>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!CheckEmailExistsAsync(changePasswordDto.Email).Result.Value)
            {
                return new NotFoundObjectResult(new ApiValidationErrorResponse
                { Errors = new[] { "Email address does not exist" } });
            }

            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == changePasswordDto.Email.ToLower());

            var result = await userManager.ResetPasswordAsync(user, changePasswordDto.PasswordResetToken, changePasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return new ApiResponse(200, "Password reset successfully.");
            }

            var errors = result.Errors.Select(e => e.Description).ToList();

            return BadRequest(new { Errors = errors });

        }

        [Authorize]
        [HttpGet("getusers")]
        public async Task<ActionResult<Pagination<UserDto>>> GetUsers([FromQuery] UserSpecParams userParams)
        {
            var spec = new UsersWithInstitutionAndRoleSpecification(userParams);
            var countSpec = new UsersWithFiltersForCountSpecification(userParams);

            var totalItems = await _userRepo.CountAsync(countSpec);
            var users = await _userRepo.ListAsync(spec);

            var data = mapper.Map<IReadOnlyList<UserDto>>(users);

            return Ok(new Pagination<UserDto>(userParams.PageIndex,
                userParams.PageSize, totalItems, data));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateUser(string id, UpdateUserDto updateUserDto)
        {

            var existingUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);  

            if (existingUser == null) { throw new Exception("user does not exist"); }

            existingUser.Role = updateUserDto.Role;
            existingUser.FirstName = updateUserDto.FirstName;
            existingUser.LastName = updateUserDto.LastName;
            existingUser.Institution = updateUserDto.Institution;

            var result = await userManager.UpdateAsync(existingUser);

            if (result.Succeeded)
            {
                return new ApiResponse(200, "User updated successfully");
            }

            var errors = result.Errors.Select(e => e.Description).ToList();

            return BadRequest(new { Errors = errors });

        }
    }
}
