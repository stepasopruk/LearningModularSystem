using System;
using Automation.Core.Templates.Data;
using UnityEditor;
using UnityEngine;

namespace Automation.Core.Templates.Window.Item
{
    public class TemplateView
    {
        public event Action LoadClick;
        public event Action UpdateClick;
        
        public void Display(Template _template, string version)
        {
            GUIStyle versionStyle = _template.IsLastVersionInstalled ? Styles.TemplateViewStyle.CorrectText : Styles.TemplateViewStyle.WrongText;

            GUILayout.BeginHorizontal();
            GUILayout.Label(_template.Name, EditorStyles.boldLabel, GUILayout.Width(80));
            GUILayout.Label(version, versionStyle, GUILayout.Width(55));
            GUILayout.Label(_template.LastVersion, Styles.TemplateViewStyle.CorrectText,GUILayout.Width(55));
            GUILayout.Space(20);

            var text = _template.Installed ? "Remove" : "Download";
            
            if (GUILayout.Button(text, GUILayout.Width(105))) 
                LoadClick?.Invoke();

            GUILayout.Space(20);
            
            if (GUILayout.Button("Update", GUILayout.Width(105))) 
                UpdateClick?.Invoke();

            GUILayout.EndHorizontal();
        }
    }
}