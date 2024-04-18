using UnityEngine;

public class EditTrainingInteractiveModule : EditModuleBase
{
    public override ModuleType ModuleType => ModuleType.InteractiveTraining;

    public override void EditModule(IModule module)
    {
        Debug.Log($"Edit {module.Name}");
    }
}
