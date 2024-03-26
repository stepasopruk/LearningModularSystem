using System.Threading.Tasks;
using Automation.Core.Templates.Data;

namespace Automation.Core.Templates.Workers.Interfaces
{
    public interface IPluginChecker
    {
        Task<Template> GetPluginInfo(string pluginName, string remoteRepository);
    }
}