using System.Threading.Tasks;
using Automation.Core.Templates.Data;

namespace Automation.Core.Templates.Workers.Interfaces
{
    public interface IPluginImporter
    {
        Task<bool> Install(Template template);
    }
}