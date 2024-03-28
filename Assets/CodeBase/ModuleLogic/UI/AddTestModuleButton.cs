using UnityEngine;
using UserInterfaceExtension;

public class AddTestModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;

    protected override void OnClick()
    {
        IModule module = new TestModule();
        moduleController.AddModuleLesson(module);
    }
}
