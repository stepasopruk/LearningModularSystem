#if UserInterfaceExtension
using LoadSceneManager.Data;
using UnityEngine;
using UserInterfaceExtension;

namespace LoadSceneManager.Utils
{
    public class LoadSceneButton : AbstractButtonView
    { 
        [SerializeField, HideInInspector] private SceneInfo _sceneInfo;

        protected override void OnClick() => LoadSceneManager.LoadScene(_sceneInfo.Name);
    }
}
#endif