using UnityEngine;
using UserInterfaceExtension;

public class AddVideoModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;

    protected override void OnClick()
    {
        IModule module = new VideoModule();
        moduleController.AddModuleLesson(module);
    }
}
