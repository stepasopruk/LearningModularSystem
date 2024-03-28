using UnityEngine;
using UserInterfaceExtension;

public class AddTextModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;

    protected override void OnClick()
    {
        IModule module = new TextModule();
        moduleController.AddModuleLesson(module);
    }
}
