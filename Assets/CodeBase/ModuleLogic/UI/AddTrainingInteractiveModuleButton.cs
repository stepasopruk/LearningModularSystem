using UnityEngine;
using UserInterfaceExtension;

public class AddTrainingInteractiveModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;

    protected override void OnClick()
    {
        IModule module = new TrainingInteractiveModule();
        moduleController.AddModuleLesson(module);
    }
}
