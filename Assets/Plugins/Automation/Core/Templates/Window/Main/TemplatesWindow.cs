using System;
using System.Threading.Tasks;
using Automation.Core.InstallModule.Git;
using Automation.Core.Templates.Window.Tab;
using Automation.Core.Templates.Workers;
using Automation.Git;
using Automation.Process;
using Automation.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace Automation.Core.Templates.Window.Main
{
    public class TemplatesWindow : EditorWindow
    {
        private static bool s_isTotalSelected;
        private static bool s_IsTotalSelected
        {
            get => s_isTotalSelected;
            set => SwitchSelectedElement(value);
        }
        
        private static TemplatesWindow s_window;
        private static TemplateTab[] s_tabControllers;
        private static TemplateManager s_templateManager;
        private static TemplateWindowView s_templateWindowView;
        
        private static int s_selectedTab;
        private static bool s_initialized;
        private static string[] s_tabNames;
        
        [MenuItem("Automation/Open Window", priority = 0)]
        public static async void OpenWindow()
        {
            s_initialized = false;
            
            if (s_window == null)
                s_window = GetWindow<TemplatesWindow>();

            s_window.minSize = new Vector2(520, 350);
            s_window.maxSize = new Vector2(520, 350);

            s_window.Show();
            
            await InitializeTemplates();

            s_initialized = true;
        }
        
        public static void SetLoading(bool status, string reason = "")
        {
            s_templateWindowView.SetReason(reason);
            s_initialized = !status;
        }

        private static async Task InitializeTemplates()
        {
            s_templateWindowView = new TemplateWindowView("обновление информации о плагинах");

            var configLoader = new ConfigLoader();
            RawTabCollection gitPath = await configLoader.GetTemplates();

            s_templateManager = new TemplateManager(new GitWorker(new ProcessStarter()), gitPath);
            Tab.Tab[] tab = await s_templateManager.GetTemplates();
            
            s_tabControllers = new TemplateTab[tab.Length];
            s_tabNames = new string[s_tabControllers.Length];
            
            for (var i = 0; i < s_tabControllers.Length; i++)
            {
                s_tabControllers[i] = new TemplateTab(tab[i], s_templateManager);
                s_tabNames[i] = s_tabControllers[i].Name;
            }
        }

         private void OnGUI()
         {
             try
             {
                 if (s_initialized)
                     DisplayTemplates();
                 else
                     s_templateWindowView.LoaderTick(Repaint);
             }
             catch (Exception e)
             {
                 CustomDebug.LogDeep(nameof(TemplatesWindow), e.Message);
                 
                 GetWindow<EditorWindow>().Close();
                 OpenWindow();
             }
         }
         
         private static void DisplayTemplates()
         {
             s_selectedTab = GUILayout.Toolbar(s_selectedTab, s_tabNames);
             
             GUILayout.BeginHorizontal();
             
             if (s_IsTotalSelected)
                 s_IsTotalSelected = s_tabControllers[s_selectedTab].IsAllSelected();

             s_IsTotalSelected = EditorGUILayout.Toggle(s_IsTotalSelected, GUILayout.Width(30));
             s_templateWindowView.DrawTopStaticText();
             GUILayout.EndHorizontal();
             
             HideTabsExcluded(s_selectedTab);
             s_tabControllers[s_selectedTab].Display();
             
             if (GUILayout.Button("Обновить выделенное")) 
                 s_tabControllers[s_selectedTab].UpdateSelected();
         }

         private static void HideTabsExcluded(int index)
         {
             for (var i = 0; i < s_tabControllers.Length; i++)
             {
                 if (index != i)
                     s_tabControllers[i].Hide();
             }
         }
         
         private static void SwitchSelectedElement(bool selected)
         {
             if (s_isTotalSelected == selected)
                 return;

             s_isTotalSelected = selected;
             s_tabControllers[s_selectedTab].SetSelectedToAll(selected);
         }
    }
}
