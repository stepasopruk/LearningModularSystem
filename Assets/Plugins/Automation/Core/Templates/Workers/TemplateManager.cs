using System.Threading.Tasks;
using Automation.Core.InstallModule.Git;
using Automation.Core.Templates.Data;
using Automation.Core.Templates.Window.Tab;
using Automation.Core.Templates.Workers.Interfaces;
using Automation.Git;

namespace Automation.Core.Templates.Workers
{
    public class TemplateManager
    {
        private readonly IPluginUpdater _pluginUpdater;
        private readonly IPluginRemover _pluginRemover;
        private readonly IPluginImporter _pluginImporter;
        private readonly TabsFactory _tabsFactory;
        
        private Tab[] _tabs;
        
        public TemplateManager(GitWorker gitWorker, RawTabCollection rawTabCollection)
        {
            var pluginsHandler =  new PluginsHandler(gitWorker);
            
            _pluginUpdater = pluginsHandler;
            _pluginImporter = pluginsHandler;
            _pluginRemover = pluginsHandler;
            
            var pluginChecker = new PluginDetector(rawTabCollection, gitWorker);
            _tabsFactory = new TabsFactory(rawTabCollection, pluginChecker);
        }
        
        public async Task<Tab[]> GetTemplates() => 
            _tabs ??= await _tabsFactory.GetTabs();


        public async Task Update(Template template)
        {
            var result = await _pluginUpdater.Update(template);
        }

        public async Task Install(Template template)
        {
            var result = await _pluginImporter.Install(template);
        }

        public async Task Remove(Template template)
        {
            var result = await _pluginRemover.Remove(template);
        }
    }
}