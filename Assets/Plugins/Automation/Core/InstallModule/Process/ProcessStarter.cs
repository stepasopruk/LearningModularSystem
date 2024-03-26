using System;
using System.Diagnostics;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Automation.Process
{
    public class ProcessStarter : IProcessStarter
    {
        public async Task<string> Start(ProcessStartInfo processInfo)
        {
            using var process = System.Diagnostics.Process.Start(processInfo);
            var result = string.Empty;
            
            if (process == null)
                return "Cant start git process";

            try
            {
                using var errorReader = process.StandardError;
                result += await errorReader.ReadToEndAsync();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception) { } 

            try
            {
                using var outputReader = process.StandardOutput;
                result += await outputReader.ReadToEndAsync();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception) { }

            return result;
        }
    }
}