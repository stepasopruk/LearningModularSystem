using UnityEngine;

public class EditTestModule : EditModuleBase
{
    public override ModuleType ModuleType => ModuleType.Test;

    public override void EditModule(IModule module)
    {
        Debug.Log($"Edit {module.Name}");
    }
}
