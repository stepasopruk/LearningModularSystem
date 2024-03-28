using UnityEngine;
using UserInterfaceExtension;

public class AddInteractiveModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;

    protected override void OnClick()
    {
        IModule module = new InteractiveModule();
        moduleController.AddModuleLesson(module);
    }
}
