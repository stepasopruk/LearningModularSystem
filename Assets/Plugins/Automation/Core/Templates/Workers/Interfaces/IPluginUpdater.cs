using System.Threading.Tasks;
using Automation.Core.Templates.Data;

namespace Automation.Core.Templates.Workers.Interfaces
{
    public interface IPluginUpdater
    {
        Task<bool>  Update(Template template);

        Task<bool> Update(Template template, string version);
    }
}