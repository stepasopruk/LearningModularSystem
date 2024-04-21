using UnityEngine;
using UserInterfaceExtension;

public class AddTrainingInteractiveModuleButton : AbstractButtonView
{
    [SerializeField] private ModuleController moduleController;
    [SerializeField] private TrainingInteractiveModuleSO moduleData;
    [SerializeField] private Sprite sprite;

    protected override void OnClick()
    {
        IModule module = new TrainingInteractiveModule(moduleData, sprite);
        moduleController.AddModuleLesson(module);
    }
}
