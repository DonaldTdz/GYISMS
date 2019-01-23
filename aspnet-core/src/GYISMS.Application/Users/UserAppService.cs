using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Session;
using GYISMS.Authorization;
using GYISMS.Authorization.Roles;
using GYISMS.Authorization.Users;
using GYISMS.Roles.Dto;
using GYISMS.Users.Dto;
using GYISMS.Employees;

namespace GYISMS.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEmployeeManager _employeeManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IEmployeeManager employeeManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _employeeManager = employeeManager;
        }

        public override async Task<UserDto> Create(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.IsEmailConfirmed = true;

            //系统用户绑定内部员工 获取用户区县信息 add by donald 2019-1-23
            if (!string.IsNullOrEmpty(user.EmployeeId))
            {
                var area = await _employeeManager.GetAreaCodeByUserIdAsync(user.EmployeeId);
                user.Area = area == GYEnums.AreaCodeEnum.None ? "" : area.ToString();
                user.AreaCode = area;
            }

            CheckErrors(await _userManager.CreateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            //系统用户绑定内部员工 获取用户区县信息 add by donald 2019-1-23
            if (!string.IsNullOrEmpty(input.EmployeeId) && input.EmployeeId != user.EmployeeId)
            {
                var area = await _employeeManager.GetAreaCodeByUserIdAsync(input.EmployeeId);
                input.Area = area == GYEnums.AreaCodeEnum.None ? "" : area.ToString();
                input.AreaCode = area;
            }

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            return await Get(input);
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password">新密码</param>
        /// <returns></returns>
        public async Task GYUpdatePassword(string password)
        {
            var userId = AbpSession.GetUserId();
            var user = _userManager.GetUserByIdAsync(userId).Result;
            user.Password = _passwordHasher.HashPassword(user, password);
            await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// 检查输入的原密码与数据库中密码是否相等
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckOldPassword(string oldPassword)
        {
            var userId = AbpSession.GetUserId();
            var entity = await _userManager.GetUserByIdAsync(userId);
            var compareResult = _passwordHasher.VerifyHashedPassword(entity, entity.Password, oldPassword);
            var result = compareResult == PasswordVerificationResult.Success ? true : false;
            return result;
        }
    }
}
