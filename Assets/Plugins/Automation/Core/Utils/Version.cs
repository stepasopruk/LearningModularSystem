using Automation.Runtime.Utils;
using UnityEditor;

namespace Automation.Core.Utils
{
    public static class Version
    {
        private const string _version = "1.3.1";
        
        [MenuItem("Automation/Version/" + _version)]
        public static void DisplayVersion() => DebugVersion();

        private static void DebugVersion()
        {
            var ver = $"Automation ver. {_version}".SetColor("blue", true);
            UnityEngine.Debug.Log(ver);
        }
    }
}