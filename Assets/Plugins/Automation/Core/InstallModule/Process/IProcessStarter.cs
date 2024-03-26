using System.Diagnostics;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Automation.Process
{
    public interface IProcessStarter
    {
        Task<string> Start(ProcessStartInfo processInfo);
    }
}