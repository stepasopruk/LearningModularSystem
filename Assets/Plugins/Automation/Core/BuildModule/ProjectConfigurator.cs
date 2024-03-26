using System;
using System.IO;
using System.Linq;
using Automation.Core.InstallModule.Utils;
using UnityEditor;
using UnityEngine;

namespace Automation.Core.BuildModule
{
    public class ProjectConfigurator
    {
        private const string ASSETS_FOLDER_NAME = "assets";
        private const string PROJECT_CONFIG_NAME = "ProjectConfig.json";
        private const string JSON_EXTENSION = ".json";
        private void Configure(ProjectConfig config)
        {
            if (config == null)
                throw new NullReferenceException($"{nameof(ProjectConfigurator)}.{nameof(Configure)} {nameof(ProjectConfig)}");
            
            Debug.Log($"Configure project to \n{config}");
        
            PlayerSettings.companyName = config.CompanyName;
            PlayerSettings.productName = config.ProductName;
            PlayerSettings.bundleVersion = config.Version;
            PlayerSettings.SetScriptingBackend(config.BuildTargetGroup, config.ScriptingImplementation);
            DefineScriptingModifier.AddDefine(config.Defines);
            EditorUserBuildSettings.SwitchActiveBuildTarget(config.BuildTargetGroup, config.BuildTarget);
            EditorUserBuildSettings.development = config.DevelopmentBuild;
        }

        public ProjectConfig Configure()
        {
            var config = GetConfig();
        
            if (config != null)
                Configure(config);
            
            return config;
        }

        private ProjectConfig GetConfig()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
                return null;
        
            var splitPath = Application.dataPath.Split(Path.AltDirectorySeparatorChar);

            var path = string.Empty;
            
            foreach (string s in splitPath)
            {
                Debug.Log(s);
                
                if (s.ToLower() == ASSETS_FOLDER_NAME)
                    break;

                if (string.IsNullOrEmpty(path))
                    path = s + Path.DirectorySeparatorChar;
                else
                    path = Path.Combine(path, s);
            }
        
            if (File.Exists(Path.Combine(path, PROJECT_CONFIG_NAME + JSON_EXTENSION)))
            {
                path = Path.Combine(path, PROJECT_CONFIG_NAME + JSON_EXTENSION);
                return ConvertFromFile(path);
            }

            if (!File.Exists(Path.Combine(path, PROJECT_CONFIG_NAME)))
                return null;
            
            path = Path.Combine(path, PROJECT_CONFIG_NAME);
            return ConvertFromFile(path);

        }

        private ProjectConfig ConvertFromFile(string path)
        {
            var jsonLines = File.ReadAllLines(path);
            var configJson = jsonLines.Aggregate(string.Empty, (current, line) => current + line);
            return JsonUtility.FromJson<ProjectConfig>(configJson);
        }
    }
}