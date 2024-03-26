using UnityEditor;
using UnityEngine;

namespace LoadSceneManager.Editor
{
    public abstract class SceneNamesFieldEditor : UnityEditor.Editor
    {
        private SerializedProperty sceneNameProperty;
        private string[] sceneNames;
        
        protected abstract string PropertyPath { get; }
        protected abstract string PopupHeader { get; }

        private void OnEnable()
        {
            sceneNameProperty = serializedObject.FindProperty(PropertyPath);
            UpdateSceneList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var prop = serializedObject.GetIterator();

            while (prop.NextVisible(true))
            {
                if (prop == sceneNameProperty)
                    continue;
                
                EditorGUILayout.PropertyField(prop);

                if (prop.isArray)
                {
                    int size = prop.arraySize;
                    
                    for (int i = 0; i < size; i++) 
                        prop.NextVisible(true);

                    prop.NextVisible(true);
                }
            }
            
            EditorGUILayout.Space(10);
            
            int selectedSceneIndex = EditorGUILayout.Popup(PopupHeader, GetSelectedSceneIndex(), sceneNames);
            
            if (selectedSceneIndex != GetSelectedSceneIndex())
            {
                sceneNameProperty.stringValue = sceneNames[selectedSceneIndex];
                serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateSceneList()
        {
            int sceneCount = EditorBuildSettings.scenes.Length;
            sceneNames = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
            }
        }

        private int GetSelectedSceneIndex()
        {
            string sceneName = sceneNameProperty.stringValue;
            return Mathf.Max(0, System.Array.IndexOf(sceneNames, sceneName));
        }
    }
}