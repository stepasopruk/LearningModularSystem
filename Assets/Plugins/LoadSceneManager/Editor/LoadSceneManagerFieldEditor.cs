using UnityEditor;

namespace LoadSceneManager.Editor
{
    [CustomEditor(typeof(LoadSceneManager))]
    public class LoadSceneManagerFieldEditor : SceneNamesFieldEditor
    {
        protected override string PropertyPath => "_nextScene.Name";
        protected override string PopupHeader => "FirstScene:";
    }
}