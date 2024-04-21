using UnityEngine;
using UserInterfaceExtension;

public class AddTestModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;
    [SerializeField] private TestModuleSO moduleData;
    [SerializeField] private Sprite sprite;

    protected override void OnClick()
    {
        IModule module = new TestModule(moduleData, sprite);
        moduleController.AddModuleLesson(module);
    }
}
