using System.Linq;
using UnityEditor;

// ReSharper disable MemberCanBePrivate.Global
namespace Automation.Core.InstallModule.Utils
{
    public static class DefineScriptingModifier
    {
        private const char SplitChar = ';';
            
        public static void UpdateDefine(string[] toAdd, string[] toRemove)
        {
            if (toAdd != null)
            {
                foreach (var def in toAdd) 
                    AddDefine(def);
            }
        
            if (toRemove != null)
            {
                foreach (var def in toRemove) 
                    RemoveDefine(def);
            }
        }
    
        public static void UpdateDefine(string toAdd, string toRemove = default)
        {
            AddDefine(toAdd);
            RemoveDefine(toRemove);
        }

        public static void AddDefine(string[] defines)
        {
            foreach (var define in defines) 
                AddDefine(define);
        }
        
        public static void AddDefine(string toAdd)
        {
            var buildTargetGroup = GetDefineConfiguration(out var defines);

            var updatedDefine = string.Empty;
            
            if (!string.IsNullOrEmpty(toAdd))
                updatedDefine = AddDefineString(defines, toAdd);
        
            if (updatedDefine != defines)
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, updatedDefine);
        }
        
        public static void RemoveDefine(string toRemove)
        {
            BuildTargetGroup buildTargetGroup = GetDefineConfiguration(out var defines);

            var updatedDefine = string.Empty;
            
            if (!string.IsNullOrEmpty(toRemove))
                 updatedDefine = RemoveDefineString(defines, toRemove);
        
            if (updatedDefine != defines)
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, updatedDefine);
        }

        public static bool ExistDefine(string define)
        {
            GetDefineConfiguration(out var defines);
            var splitDefines = defines.Split(SplitChar);

            return splitDefines.Contains(define);
        }

        private static string AddDefineString(string defines, string additional)
        {
            var definesList = defines.Split(SplitChar);

            return definesList.Length > 0
                ? definesList.Contains(additional) ? defines : defines + SplitChar + additional
                : additional;
        }

        private static string RemoveDefineString(string defines, string removable)
        {
            var definesList = defines.Split(SplitChar);
            return definesList.Contains(removable) ? defines.Replace(removable, string.Empty) : defines;
        }

        private static BuildTargetGroup GetDefineConfiguration(out string defines)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return buildTargetGroup;
        }
    }
}