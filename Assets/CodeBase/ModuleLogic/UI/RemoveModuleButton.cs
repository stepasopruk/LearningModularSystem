using UnityEngine;
using UserInterfaceExtension;

public class RemoveModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;

    protected override void Awake()
    {
        base.Awake();
        moduleController.ToggleActive += ModuleController_ToggleActive;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        moduleController.ToggleActive -= ModuleController_ToggleActive;
    }

    protected override void OnClick() => 
        moduleController.RemoveModuleLesson();

    private void ModuleController_ToggleActive(bool active) =>
    _button.interactable = active;
}
