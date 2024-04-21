using UnityEngine;
using UserInterfaceExtension;

public class AddTextModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;
    [SerializeField] private TextModuleSO moduleData;
    [SerializeField] private Sprite sprite;

    protected override void OnClick()
    {
        IModule module = new TextModule(moduleData, sprite);
        moduleController.AddModuleLesson(module);
    }
}
