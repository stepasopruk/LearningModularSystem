using System.Linq;
using Automation.Core.Templates.Data;
using Automation.Core.Templates.Window.Item;
using Automation.Core.Templates.Workers;
using UnityEditor;
using UnityEngine;

namespace Automation.Core.Templates.Window.Tab
{
    public class TemplateTab
    {
        public string Name => _tab.Name;
        
        private readonly Tab _tab;
        private readonly TemplateManager _templateManager;
        private readonly EditorElement[] _elements;
        
        private Vector2 _scrollPosition;
        
        public TemplateTab(Tab tab, TemplateManager templateManager)
        {
            _templateManager = templateManager;
            
            _tab = tab;
            
            _elements = new EditorElement[_tab.Templates.Length];
            
            Initialize();
        }

        public void Display()
        {
            foreach (EditorElement element in _elements) 
                element.Display();
        }

        public void Hide()
        {
            _scrollPosition = Vector2.zero;

            for (var i = 0; i < _elements.Length; i++) 
                _elements[i].Selected = false;
        }

        public void SetSelectedToAll(bool selected)
        {
            for (var i = 0; i < _elements.Length; i++) 
                _elements[i].Selected = selected;
        }

        public bool IsAllSelected() => _elements.All(e => e.Selected);

        private void Initialize()
        {
            int index = 0;
            foreach (Template template in _tab.Templates)
            {
                var templateController = new TemplateController(template, _templateManager);
                _elements[index] = new EditorElement(templateController);
                index++;
            }
        }

        public void UpdateSelected()
        {
            foreach (EditorElement element in _elements.Where(e => e.Selected))
            {
                element.UpdateOrLoad();
            }
        }
        
        private class EditorElement
        {
            public bool Selected;

            private readonly TemplateController TemplateController;

            public EditorElement(TemplateController templateController)
            {
                TemplateController = templateController;
            }

            public void UpdateOrLoad()
            {
                if (TemplateController.IsTemplateInstalled)
                    TemplateController.Update();
                else
                    TemplateController.Load();
            }

            public void Display()
            {
                GUILayout.BeginHorizontal();
                Selected = EditorGUILayout.Toggle(Selected, GUILayout.Width(30));
                TemplateController.Display();
                GUILayout.Space(8);
                GUILayout.EndHorizontal();
            }
        }
    }

    public class Tab
    {
        public string Name;
        public Template[] Templates;
    }
}