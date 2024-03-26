using System;
using UnityEditor;
using UnityEngine;

namespace Automation.Core.Templates.Window.Main
{
    public class TemplateWindowView
    {
        private const string DEFAULT_REASON = "загрузка";
        private const string WAIT_TEXT = "Ожидайте";
        private const double TICK_TIME = 0.5;
        
        private readonly string[] items = {"Идет {0}.", "Идет {0}..", "Идет {0}...", "Идет {0}...."};
        
        private DateTime _calculationStartTime = DateTime.Now;
        
        private int _index;
        private string _reason;

        public TemplateWindowView(string fistInitReason = "") => 
            SetReason(fistInitReason);


        public void SetReason(string reason = "")
        {
            _reason = string.IsNullOrWhiteSpace(reason) ? DEFAULT_REASON : reason;
        }
        
        public void LoaderTick(Action repaint)
        {
            GUILayout.FlexibleSpace();
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(WAIT_TEXT);
                GUILayout.FlexibleSpace();
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format(items[_index], _reason));
                GUILayout.FlexibleSpace();
            }

            GUILayout.FlexibleSpace();
            
            CalculateTick();
            
            repaint?.Invoke();
        }

        public void DrawTopStaticText()
        {
            GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(80));
            GUILayout.Space(-10);
            GUILayout.Label("Version", EditorStyles.boldLabel, GUILayout.Width(55));
            GUILayout.Space(10);
            GUILayout.Label("LST", EditorStyles.boldLabel, GUILayout.Width(55));
            GUILayout.Space(265);
        }

        private void CalculateTick()
        {
            double timeElapsed = (DateTime.Now - _calculationStartTime).TotalSeconds;
            
            if (timeElapsed >= TICK_TIME)
            {
                _calculationStartTime = DateTime.Now;
                Tick();
            }
        }

        private void Tick()
        {
            _index++;
            if (_index >= items.Length)
                _index = 0;
        }
    }
}