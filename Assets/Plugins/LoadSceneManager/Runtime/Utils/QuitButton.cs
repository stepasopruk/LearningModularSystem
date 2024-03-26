#if UserInterfaceExtension
using UnityEngine;
using UserInterfaceExtension;

namespace LoadSceneManager.Utils
{
    public class QuitButton : AbstractButtonView
    {
        protected override void OnClick()
        {
            Application.Quit();
        }
    }
}
#endif