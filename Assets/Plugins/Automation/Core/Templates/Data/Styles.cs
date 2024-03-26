using UnityEditor;
using UnityEngine;

namespace Automation.Core.Templates.Data
{
    public static class Styles
    {
        public static readonly TemplateViewStyles TemplateViewStyle = new TemplateViewStyles();

        public class TemplateViewStyles
        {
            public readonly GUIStyle CorrectText;
            public readonly GUIStyle WrongText;

            public TemplateViewStyles()
            {
                CorrectText = new GUIStyle(EditorStyles.label)
                {
                    normal =
                    {
                        textColor = Color.green
                    }
                };
                
                WrongText = new GUIStyle(EditorStyles.label)
                {
                    normal =
                    {
                        textColor = Color.red
                    }
                };
            }
        }
    }
}