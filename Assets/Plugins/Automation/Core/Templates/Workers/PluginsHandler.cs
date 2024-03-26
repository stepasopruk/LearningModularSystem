using System;
using System.IO;
using System.Threading.Tasks;
using Automation.Core.InstallModule.Git;
using Automation.Core.InstallModule.Utils;
using Automation.Core.Templates.Data;
using Automation.Core.Templates.Workers.Interfaces;
using Automation.Runtime.Utils;
using UnityEditor;

namespace Automation.Core.Templates.Workers
{
    public class PluginsHandler : IPluginImporter, IPluginUpdater, IPluginRemover
    {
        private readonly GitWorker _gitWorker;

        public PluginsHandler(GitWorker gitWorker)
        {
            _gitWorker = gitWorker;
        }

        public async Task<bool> Install(Template template)
        {
            string pluginName = template.Name;
            string gitSource = template.GitInfo.Path;
            string toPath = Path.Combine(StaticData.PluginsPath, pluginName);
                        
            if (Directory.Exists(toPath))
            {
                CustomDebug.Log(nameof(IPluginImporter), $"Folder {pluginName} exists in Plugin");
                return false;
            }
                        
            if (!Directory.Exists(StaticData.PluginsPath))
                Directory.CreateDirectory(StaticData.PluginsPath);
                        
            Directory.CreateDirectory(toPath);
            
            string tempFolder = await CloneReposToTemp(gitSource, pluginName);
                        
            MoveContent(tempFolder, toPath);

            if (!DefineScriptingModifier.ExistDefine(pluginName))
                DefineScriptingModifier.AddDefine(pluginName);
            
            AssetDatabase.Refresh();
            return true;
        }
        
        public async Task<bool> Update(Template template) => await Update(template, string.Empty);

        public async Task<bool> Update(Template template, string version)
        {
            string pluginName = template.Name;
            string gitPath = template.GitInfo.Path;
            
            string pluginFolder = Path.Combine(StaticData.PluginsPath, pluginName);
            string tempFolder = await CloneReposToTemp(gitPath, pluginName);

            if (!string.IsNullOrEmpty(version))
                await _gitWorker.CheckoutToTag(tempFolder, version);

            if (!Directory.Exists(pluginFolder))
            {
                CustomDebug.Log(nameof(IPluginUpdater),$"Plugin {pluginName} not exists in Plugin");
                return false;
            }

            Directory.Delete(pluginFolder, true);
            Directory.CreateDirectory(pluginFolder);

            MoveContent(tempFolder, pluginFolder);
            
            if (!DefineScriptingModifier.ExistDefine(pluginName))
                DefineScriptingModifier.AddDefine(pluginName);
            
            AssetDatabase.Refresh();

            return true;
        }
        
        public Task<bool> Remove(Template template)
        {
            string pluginName = template.Name;
            string pluginPath = Path.Combine(StaticData.PluginsPath, pluginName);
            string folderMetaPath = pluginPath + StaticData.META_EXTENSION;
            
            if (!Directory.Exists(pluginPath))
            {
                CustomDebug.Log(nameof(IPluginRemover), $"Plugin {pluginName} dont exist in Plugins folder");
                return Task.FromResult(false);
            }
            
            Directory.Delete(pluginPath, true);

            if (File.Exists(folderMetaPath))
                File.Delete(Path.Combine(folderMetaPath));

            if (DefineScriptingModifier.ExistDefine(pluginName))
                DefineScriptingModifier.RemoveDefine(pluginName);
            
            AssetDatabase.Refresh();
            return Task.FromResult(true);
        }

        private async Task<string> CloneReposToTemp(string gitSource, string pluginName)
        {
            var tempPluginFolderName = pluginName + $"{DateTime.Now:yyMMddmmss}";
            
            var folderTo = Path.Combine(StaticData.ProjectPath, StaticData.SubTempPath, tempPluginFolderName);
            CustomDebug.Log(nameof(PluginsHandler), $"Try clone to {folderTo}");
            CustomDebug.Log(nameof(PluginsHandler), $"Try clone to ");
            if (Directory.Exists(folderTo))
                Directory.Delete(folderTo, true);
            
            Directory.CreateDirectory(folderTo);
            
            await _gitWorker.CloneRepository(gitSource, folderTo);
            
            return folderTo;
        }
        
        private void MoveContent(string fromPath, string toPath)
        {
            var directoryInfo = new DirectoryInfo(fromPath);
            
            if (!Directory.Exists(toPath))
                Directory.CreateDirectory(toPath);

            string path;
            
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                if (directory.Name == ".git" || directory.Name == ".gitignore")
                    continue;
                
                path = Path.Combine(toPath, directory.Name);
                
                if (Directory.Exists(path))
                    Directory.Delete(path);

                directory.MoveTo(Path.Combine(toPath, directory.Name));
            }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Name == ".git" || file.Name == ".gitignore")
                    continue;

                path = Path.Combine(toPath, file.Name);
                
                if (File.Exists(path))
                    File.Delete(path);
                
                file.MoveTo(Path.Combine(toPath, file.Name));
            }
        }
    }
}