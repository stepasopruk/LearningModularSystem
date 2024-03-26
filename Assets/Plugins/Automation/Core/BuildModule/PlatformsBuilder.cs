using System.IO;
using UnityEditor;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global
namespace Automation.Core.BuildModule
{
    public class PlatformsBuilder
    {
        public void BuildWindows64() => BuildWindows64(GetDefaultPath());
         
        public void BuildWindows64(string path)
        {
            Debug.Log($"Start build in path: {path}");
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.StandaloneWindows64, BuildOptions.None);
            Debug.Log("Build finished");
        }
     
        public void BuildWindows32() => BuildWindows32(GetDefaultPath());
     
        public void BuildWindows32(string path)
        {
            Debug.Log($"Start build in path: {path}");
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.StandaloneWindows, BuildOptions.None);
            Debug.Log("Build finished");
        }
         
        public void BuildLinux() => BuildLinux(GetDefaultPath());
        public void BuildLinux(string path)
        {
            Debug.Log($"Start build in path: {path}");
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.StandaloneLinux64, BuildOptions.None);
            Debug.Log("Build finished");
        } 
         
        public void BuildAndroid() => BuildAndroid(GetDefaultPath());
        public void BuildAndroid(string path)
        {
            Debug.Log($"Start build in path: {path}");
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.Android, BuildOptions.None);
            Debug.Log("Build finished");
        }
         
        private string GetDefaultPath()
        {
            var name = $"{Application.productName}.exe";
            var folderName = $"{Application.productName}_{Application.version}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Builds", folderName, name);
            return path;
        }
    }
}