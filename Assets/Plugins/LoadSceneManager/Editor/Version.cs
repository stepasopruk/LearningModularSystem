using UnityEditor;

namespace LoadSceneManager
{
    public static class Version
    {
        private const string _version = "1.1.0";

        [MenuItem("Utilities/LoadSceneManager/Version/" + _version)]
        public static void DisplayVersion() => DebugVersion();

        private static void DebugVersion()
        {
            var ver = $"LoadSceneManager ver. {_version}".SetColor("blue", true);
            UnityEngine.Debug.Log(ver);
        }
        
        private static string SetColor(this string text, string color, bool bold) => 
            "<color=" + color + ">" + (bold ? "<b>" : "") + text + (bold ? "</b>" : "") + "</color>";
    }
}
