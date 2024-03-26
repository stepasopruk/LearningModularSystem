using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Automation.Core.Templates.Data;
using Automation.Core.Utils.Coroutines;
using Automation.Git;
using Automation.Runtime.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Automation.Core.Templates.Workers
{
    public class ConfigLoader
    {
        private bool _intCompleted;

        private CancellationTokenSource _tokenSource;
        
        public async Task<RawTabCollection> GetTemplates()
        {
            _intCompleted = IsInitialized();

            EditorCoroutineUtility.StartCoroutineOwnerless(GetRemoteConfig(() => _intCompleted = true));

            while (!_intCompleted) 
                await Task.Yield();

            return LoadPackageInfo();
        }
        
        private bool IsInitialized() => File.Exists(StaticData.TemplatesInfoConfigPath);
        

        private RawTabCollection LoadPackageInfo()
        {
            CustomDebug.Log(nameof(ConfigLoader), "Try to load JSON from local storage");
            var json = Resources.Load<TextAsset>(StaticData.GIT_PATHS_CONFIG).text;
            return JsonUtility.FromJson<RawTabCollection>(json);
        }

        private IEnumerator GetRemoteConfig(Action onComplete)
        {
            CustomDebug.Log(nameof(GetRemoteConfig), $"Send request to {StaticData.TEMPLATES_CONFIG_URI}");
            
            var request = UnityWebRequest.Get(StaticData.TEMPLATES_CONFIG_URI);
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                CustomDebug.Log(nameof(GetRemoteConfig), $"{request.error}");
                yield break;
            }
            
            var validatedJson = ValidateJsonAfterRequest(request.downloadHandler.text);
            
            CustomDebug.LogDeep(nameof(GetRemoteConfig), $"Received JSON \n {validatedJson}");

            bool updated = false;
            UpdatePackageFile(validatedJson, () => updated = true);

            CustomDebug.Log(nameof(GetRemoteConfig), "Templates config JSON updated");

            while (!updated)
                yield return null;

            onComplete?.Invoke();
        }
        
        private async void UpdatePackageFile(string json, Action completed)
        {
            var path = StaticData.TemplatesInfoConfigPath;

            var sw = new StreamWriter(path, true, Encoding.ASCII);
            sw.Close();

            var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();

            if (File.Exists(path))
            {
                string currentJson = await File.ReadAllTextAsync(path);

                if (currentJson == json) 
                    return;

#if UNITY_2021_3_OR_NEWER                
                await File.WriteAllTextAsync(path, string.Empty);
#else
                File.WriteAllText(path, string.Empty);
#endif
            }
            else
            {
                File.Create(path);
            }
#if UNITY_2021_3_OR_NEWER
            await File.WriteAllTextAsync(path, json);
#else
            File.WriteAllText(path, json);
#endif
            AssetDatabase.Refresh();
            completed?.Invoke();
        }
        
        private string ValidateJsonAfterRequest(string json)
        {
            json = json
                .Replace("\"", "")
                .Replace("\\r\\n", string.Empty)
                .Replace("\\u0022", "\"")
                .Replace("\\n", "");
            
            CustomDebug.LogDeep(nameof(ValidateJsonAfterRequest),$"Json updated to {json}");
            return json;
        }
    }
}