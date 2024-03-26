using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Automation.Core.InstallModule.Git;
using Automation.Core.Templates.Data;
using Automation.Core.Templates.Workers.Interfaces;
using Automation.Git;
using UnityEngine;

namespace Automation.Core.Templates.Workers
{
    public class PluginDetector : IPluginChecker
    {
        private readonly GitWorker _gitWorker;
        private readonly RawTabCollection _rawTabCollection;
        
        public PluginDetector(RawTabCollection rawTabCollection, GitWorker gitWorker)
        {
            _rawTabCollection = rawTabCollection;
            _gitWorker = gitWorker;
        }

        public async Task<Template> GetPluginInfo(string pluginName, string remoteRepository)
        {
            var pluginPath = Path.Combine(StaticData.PluginsPath, pluginName);
            
            var isPluginExist = Directory.Exists(StaticData.PluginsPath) && Directory.Exists(pluginPath) &&
                                Directory.GetFiles(pluginPath).Length > 0;

            if (isPluginExist == false && Directory.Exists(pluginPath))
                Debug.Log($"Обнаружена пустая папка {pluginName} в папке Plugins");
            
            var callbackResult = await _gitWorker.GetTags(remoteRepository);
            var lastTag = callbackResult.Arguments.LastOrDefault();

            var gitInfo = _rawTabCollection.Categories.SelectMany(t => t.Items).FirstOrDefault(x => x.Name == pluginName);
            return new Template(pluginName, GetVersion(pluginPath), lastTag, isPluginExist, gitInfo);
        }
        
        private string GetVersion(string pluginPath)
        {
            var filePath = Path.Combine(pluginPath, StaticData.VERSION_FILE_NAME);

            if (!Directory.Exists(pluginPath))
                return "0";
            
            return File.Exists(filePath) ? File.ReadAllText(filePath) : "unknown";
        }
    }
}