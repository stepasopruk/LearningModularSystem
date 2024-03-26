using System.Threading.Tasks;
using Automation.Core.Templates.Data;

namespace Automation.Core.Templates.Workers.Interfaces
{
    public interface IPluginRemover
    {
        Task<bool> Remove(Template template);
    }
}