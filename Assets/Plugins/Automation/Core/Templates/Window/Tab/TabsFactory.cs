using System.Threading.Tasks;
using Automation.Core.Templates.Data;
using Automation.Core.Templates.Workers.Interfaces;
using Automation.Git;

namespace Automation.Core.Templates.Window.Tab
{
    public class TabsFactory
    {
        private Tab[] _tabs;
        
        private readonly RawTabCollection _rawInfo;
        private readonly IPluginChecker _pluginChecker;

        public TabsFactory(RawTabCollection rawInfo, IPluginChecker pluginChecker)
        {
            _rawInfo = rawInfo;
            _pluginChecker = pluginChecker;
        }
        
        public async Task<Tab[]> GetTabs() => 
            _tabs ??= await InitializeTabs();

        private async Task<Tab[]> InitializeTabs()
        {
            _tabs = new Tab[_rawInfo.Categories.Length];

            for (var tabIndex = 0; tabIndex < _rawInfo.Categories.Length; tabIndex++)
            {
                RawTab rawRawTab = _rawInfo.Categories[tabIndex];
                
                _tabs[tabIndex] = new Tab
                {
                    Name = rawRawTab.Name,
                    Templates = new Template[rawRawTab.Items.Length]
                };

                for (var templateIndex = 0; templateIndex < rawRawTab.Items.Length; templateIndex++)
                {
                    _tabs[tabIndex].Templates[templateIndex] = await _pluginChecker.GetPluginInfo(rawRawTab.Items[templateIndex].Name, rawRawTab.Items[templateIndex].Path);
                }
            }

            return _tabs;
        }
    }
}