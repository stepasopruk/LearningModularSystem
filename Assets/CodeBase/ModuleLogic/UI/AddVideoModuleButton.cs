using UnityEngine;
using UserInterfaceExtension;

public class AddVideoModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;
    [SerializeField] private VideoModuleSO moduleData;
    [SerializeField] private Sprite sprite;

    protected override void OnClick()
    {
        IModule module = new VideoModule(moduleData, sprite);
        moduleController.AddModuleLesson(module);
    }
}
