using System.IO;
using System.Linq;
using Automation.Runtime.Utils;
using UnityEngine;

namespace Automation.Core.Templates.Data
{
    public static class StaticData
    {
        public const string META_EXTENSION = ".meta";
        public const string PLUGINS_FOLDER_NAME = "Plugins";
        public const string VERSION_FILE_NAME = "Version.txt";
        public const string GIT_PATHS_CONFIG = "plugins-path";
        public const string TEMPLATES_CONFIG_URI = "http://servergit/PackageManager/Home/GetInformationJson";

        public static  string TempPluginPath = ProjectPath + SubTempPath;
        public static readonly string PluginsPath = Path.Combine(Application.dataPath, PLUGINS_FOLDER_NAME);
        public static string TemplatesInfoConfigPath = Path.Combine(PluginsPath, "Automation", "Core", "Resources", $"{GIT_PATHS_CONFIG}.json");

        public static readonly string SubTempPath = Path.Combine("Temp", "PluginClones");
        private static readonly string[] SplitPathToAsset = Application.dataPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        public static readonly string ProjectPath = SplitPathToAsset.Take(SplitPathToAsset.Length - 1).FromArrayToString(Path.AltDirectorySeparatorChar);
        
    }
}