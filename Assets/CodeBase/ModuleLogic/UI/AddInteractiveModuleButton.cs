using System.Linq;
using UnityEngine;
using UserInterfaceExtension;

public class AddInteractiveModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;
    [SerializeField] private InteractiveModuleSO moduleData;
    [SerializeField] private Sprite sprite;

    protected override void OnClick()
    {
        IModule module = new InteractiveModule(moduleData, sprite);
        moduleController.AddModuleLesson(module);
    }
}
