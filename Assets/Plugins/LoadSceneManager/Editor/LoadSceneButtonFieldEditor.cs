#if UserInterfaceExtension
using LoadSceneManager.Utils;
using UnityEditor;

namespace LoadSceneManager.Editor
{
    [CustomEditor(typeof(LoadSceneButton))]
    public class LoadSceneButtonFieldEditor : SceneNamesFieldEditor
    {
        protected override string PropertyPath => "_sceneInfo.Name";
        protected override string PopupHeader => "Сцена";
    }
}
#endif