using System.Reflection;
using Automation.Core.InstallModule.Utils;
using Automation.Runtime.Utils;
using UnityEditor;

namespace Automation.Core.Utils
{
    
    public static class CustomDebugConfigurator
    {
        private const string LOG_KEY = "TemplatesDebugLevel";
        private const string DEBUG_IN_BUILD_DEFINE = "AUTOMATION_DEBUG";

        private static CustomDebug instance;
        
        static CustomDebugConfigurator()
        {
            UpdateDebugLevelView(GetDebugType());
            UpdateBuildConfigurationView(DefineScriptingModifier.ExistDefine(DEBUG_IN_BUILD_DEFINE));
        }
        
        [MenuItem("Automation/Debug/DebugLevel/Deep")]
        private static void EnableDeepDebug() => SetDebug(DebugType.Deep);

        [MenuItem("Automation/Debug/DebugLevel/Default")]
        private static void EnableDebug() => SetDebug(DebugType.Default);

        [MenuItem("Automation/Debug/DebugLevel/NoLogs")]
        private static void DisableDebug() => SetDebug(DebugType.NoLogs);

        [MenuItem("Automation/Debug/BuildSettings/EnabledInBuild")]
        private static void EnableDebugInBuild()
        {
            DefineScriptingModifier.AddDefine(DEBUG_IN_BUILD_DEFINE);
            UpdateBuildConfigurationView(true);
        }

        [MenuItem("Automation/Debug/BuildSettings/DisabledInBuild")]
        private static void DisableDebugInBuild()
        {
            DefineScriptingModifier.RemoveDefine(DEBUG_IN_BUILD_DEFINE);
            UpdateBuildConfigurationView(false);
        }
        
        
        private static void SetDebug(DebugType debugType)
        {
            EditorPrefs.SetInt(LOG_KEY, (int) debugType);

            UpdateDebugLevelView(debugType);
        }
        
        private static void UpdateDebugLevelView(DebugType debugType)
        {
            Menu.SetChecked("Automation/Debug/DebugLevel/Deep", debugType == DebugType.Deep);
            Menu.SetChecked("Automation/Debug/DebugLevel/Default", debugType == DebugType.Default);
            Menu.SetChecked("Automation/Debug/DebugLevel/NoLogs", debugType == DebugType.NoLogs);
        }

        private static void UpdateBuildConfigurationView(bool isEnabled)
        {
            Menu.SetChecked("Automation/Debug/BuildSettings/EnabledInBuild", isEnabled);
            Menu.SetChecked("Automation/Debug/BuildSettings/DisabledInBuild", !isEnabled);
        }


        private static DebugType GetDebugType()
        {
            MethodInfo method = typeof(CustomDebug).GetMethod("GetDebugType", BindingFlags.Static | BindingFlags.NonPublic);
            object result = method?.Invoke(null, null);
            return (DebugType)(result ?? DebugType.NoLogs);
        }
    }
}
