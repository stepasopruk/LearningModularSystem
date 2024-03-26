using System;
using System.Threading;
using System.Threading.Tasks;
using Automation.Core.Templates.Data;
using Automation.Core.Templates.Window.Main;
using Automation.Core.Templates.Workers;

namespace Automation.Core.Templates.Window.Item
{
    public class TemplateController
    {
        private const int BEFORE_ACTION_DELAY_IN_SECONDS = 1;
        private const int AFTER_ACTION_DELAY_IN_SECONDS = 5;
        
        private readonly Template _template;
        private readonly TemplateManager _templateManager;
        private readonly CancellationTokenSource _tokenSource;
        public bool IsTemplateInstalled => _template.Installed;
        
        private TemplateView _templateView;

        public TemplateController(Template template, TemplateManager templateManager)
        {
            _template = template;
            _templateManager = templateManager;
            _tokenSource = new CancellationTokenSource();

            _templateView = new TemplateView();
            
            _templateView.LoadClick += TemplateView_OnLoadClick;
            _templateView.UpdateClick += TemplateView_OnUpdateClick;
        }

        public async void Load()
        {
            TemplatesWindow.SetLoading(true, $"загрузка {_template.Name}");
            
            await _templateManager.Install(_template);
            await Task.Delay(TimeSpan.FromSeconds(10), _tokenSource.Token);
            
            TemplatesWindow.SetLoading(false);
        }
        
        public void Display()
        {
            _templateView.Display(_template, FormatVersion());
        }

        public async void Update()
        {
            TemplatesWindow.SetLoading(true, $"обновление {_template.Name}");
            await _templateManager.Update(_template);
            await Task.Delay(TimeSpan.FromSeconds(AFTER_ACTION_DELAY_IN_SECONDS), _tokenSource.Token);
            
            TemplatesWindow.SetLoading(false);
        }

        private async void Remove()
        {
            TemplatesWindow.SetLoading(true, $"удаление {_template.Name}");
            
            await Task.Delay(TimeSpan.FromSeconds(BEFORE_ACTION_DELAY_IN_SECONDS), _tokenSource.Token);
            await _templateManager.Remove(_template);
            await Task.Delay(TimeSpan.FromSeconds(AFTER_ACTION_DELAY_IN_SECONDS), _tokenSource.Token);
            
            TemplatesWindow.SetLoading(false);
        }
        
        private void TemplateView_OnUpdateClick()
        {
            Update();
        }

        private void TemplateView_OnLoadClick()
        {
            if (_template.Installed)
                Remove();
            else 
                Load();
        }

        private string FormatVersion() => 
            _template.Version == "0" ? "unins.." : _template.Version;

        ~TemplateController()
        {
            TemplatesWindow.SetLoading(false);
            _tokenSource.Dispose();
        }
    }
}