using System;
using UnityEditor;
using UnityEngine;

namespace Automation.Runtime.Utils
{
    public class CustomDebug
    {
        public static void Log(string sender, string message, string color = "#000080ff", bool bold = true)
        {
            if (GetDebugType() == DebugType.NoLogs)
                return;
            
            var senderCustomized = $"[{sender}]".SetColor(color, bold);
            Debug.Log($"{senderCustomized}: {message}");
        }
        
        [Obsolete]
        public static void Log(string message)
        {
            if (GetDebugType() == DebugType.NoLogs)
                return;
            
            Debug.Log(message);
        }

        public static void LogDeep(string sender, string message, string color = "green", bool bold = true)
        {
            if (GetDebugType() != DebugType.Deep)
                return;
            
            var senderCustomized = $"[{sender}]".SetColor(color, bold);
            Debug.Log($"{senderCustomized}: {message}");
        }
        
        private static DebugType GetDebugType()
        {
#if AUTOMATION_DEBUG && !UNITY_EDITOR
            return DebugType.Deep;
#endif
            
#if UNITY_EDITOR
            return (DebugType)EditorPrefs.GetInt("TemplatesDebugLevel", (int)DebugType.Default);
#else
            return DebugType.NoLogs;
#endif           
        }
    }
}