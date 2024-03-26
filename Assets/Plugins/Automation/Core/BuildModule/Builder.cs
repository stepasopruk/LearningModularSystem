using System;
using UnityEditor;

namespace Automation.Core.BuildModule
{
    public class Builder
    {
        private static readonly ProjectConfigurator _projectConfigurator = new ProjectConfigurator();
        private static readonly PlatformsBuilder _platformsBuilder = new PlatformsBuilder();
    
        public static void Begin()
        {
            var config = _projectConfigurator.Configure();

            if (config == null)
                throw new Exception($"{nameof(Builder)} error: {nameof(ProjectConfig)} is null/not found/not valid");
            
            Build(config.BuildTarget);
        }
        
        private static void Build(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows :
                    _platformsBuilder.BuildWindows64();
                    break;
                case BuildTarget.StandaloneWindows64 :
                    _platformsBuilder.BuildWindows32();
                    break;
                case BuildTarget.StandaloneLinux64 :
                    _platformsBuilder.BuildLinux();
                    break;
                case BuildTarget.Android :
                    _platformsBuilder.BuildAndroid();
                    break;
                default:
                    throw new Exception($"{nameof(Builder)} error: Build target {buildTarget} not supported");
            }
        }
    }
}