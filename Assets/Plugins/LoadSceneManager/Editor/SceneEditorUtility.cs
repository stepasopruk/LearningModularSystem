using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadSceneManager.Editor
{
    public class SceneEditorUtility : EditorWindow
    {
        [MenuItem("Utilities/LoadSceneManager/Scene Manager")]
        public static void CreatingWindow()
        {
            GetWindow<SceneEditorUtility>("Scene Manager");
        }

        private void OnGUI()
        {
            var scenePaths = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToList();

            var currentScene = SceneManager.GetActiveScene();
        
            foreach (var sceneName in scenePaths)
            {
                GUILayout.BeginHorizontal();

                GUI.backgroundColor = currentScene == SceneManager.GetSceneByPath(sceneName) ? Color.green : Color.white;
                
                if (GUILayout.Button(Path.GetFileNameWithoutExtension(sceneName)))
                {
                    SaveScenes(scenePaths);
                    OpenSceneSingle(sceneName);
                }

                GUI.backgroundColor = SceneManager.GetSceneByPath(sceneName).name != null ? Color.yellow : Color.white;
                
                if (GUILayout.Button("Additive"))
                {
                    if (SceneManager.GetSceneByPath(sceneName).name == null)
                        OpenSceneAdditive(sceneName);
                    else
                        CloseAdditiveSceneAndSave(sceneName);
                }
                GUILayout.EndHorizontal();
                
                GUI.backgroundColor = Color.white;
            }

            if (GUILayout.Button("Запустить с первой сцены.") && scenePaths.Any())
            {
                SaveScenes(scenePaths);
                OpenSceneSingle(scenePaths.First());
                EditorApplication.isPlaying = true;
            }
        }

        private static void CloseAdditiveSceneAndSave(string sceneName)
        {
            EditorSceneManager.SaveScene(SceneManager.GetSceneByPath(sceneName));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByPath(sceneName));
        }

        private static void OpenSceneAdditive(string sceneName)
        {
            EditorSceneManager.OpenScene(sceneName, OpenSceneMode.Additive);
        }

        private static void OpenSceneSingle(string scenes)
        {
            EditorSceneManager.OpenScene(scenes, OpenSceneMode.Single);
        }

        private static void SaveScenes(IEnumerable<string> scenePaths)
        {
            foreach (var path in scenePaths) 
                EditorSceneManager.SaveScene(SceneManager.GetSceneByPath(path));
        }
    }
}