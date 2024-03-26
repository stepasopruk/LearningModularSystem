using UnityEditor;

namespace Automation.Core.Utils
{
    [InitializeOnLoad]
    public static class TemplatesDebugLevel
    {
        private const string LOG_KEY = "TemplatesDebugLevel";
        public static bool IsDebugEnabled => GetDebug();

        static TemplatesDebugLevel()
        {
            Menu.SetChecked("Automation/Debug/Off", !IsDebugEnabled);    
            Menu.SetChecked("Automation/Debug/On", IsDebugEnabled);    
        }

        [MenuItem("Automation/Debug/On")]
        public static void EnableDebug() => SetDebug(true);

        [MenuItem("Automation/Debug/Off")]
        public static void DisableDebug() => SetDebug(false);

        private static void SetDebug(bool isDebug)
        {
            EditorPrefs.SetBool(LOG_KEY, isDebug);
            
            Menu.SetChecked("Automation/Debug/Off", !IsDebugEnabled);    
            Menu.SetChecked("Automation/Debug/On", IsDebugEnabled); 
        }

        private static bool GetDebug() => EditorPrefs.GetBool(LOG_KEY, true);
    }
}