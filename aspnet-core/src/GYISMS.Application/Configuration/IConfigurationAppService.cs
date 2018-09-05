using System.Threading.Tasks;
using GYISMS.Configuration.Dto;

namespace GYISMS.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
