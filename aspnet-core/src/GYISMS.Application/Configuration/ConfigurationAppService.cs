using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using GYISMS.Configuration.Dto;

namespace GYISMS.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : GYISMSAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
