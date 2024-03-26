using System;
using UnityEditor;

namespace Automation.Core.BuildModule
{
    [Serializable]
    public class ProjectConfig
    {
        public string ProductName = "AnatomicAtlas";
        public string CompanyName = "BK-Studio";
        public string Version = "1.0.0";
        
        public BuildTargetGroup BuildTargetGroup = BuildTargetGroup.Standalone;
        public BuildTarget BuildTarget = BuildTarget.StandaloneWindows64;
        public ScriptingImplementation ScriptingImplementation = ScriptingImplementation.Mono2x;

        public string[] Defines = Array.Empty<string>();
        
        public bool DevelopmentBuild = false;

        public override string ToString()
        {
            var defines = string.Empty;
            for (var i = 0; i < Defines.Length; i++)
            {
                if (i == 0)
                {
                    defines += Defines[i];
                    continue;
                }

                defines += " " + Defines[i];
            }
        
            return $"{nameof(ProductName)}: {ProductName} \n{nameof(CompanyName)}: {CompanyName} \n{nameof(Version)}: {Version} " +
                   $"\n{nameof(BuildTarget)}: {BuildTarget} \n{nameof(BuildTargetGroup)}: {BuildTargetGroup} " +
                   $"\n{nameof(BuildTargetGroup)}: {BuildTargetGroup} \n" + $"{nameof(ScriptingImplementation)}: {ScriptingImplementation} " +
                   $"\n{nameof(Defines)}: {defines} \n" + $"{nameof(DevelopmentBuild)}: {DevelopmentBuild}";
        }
    }
}