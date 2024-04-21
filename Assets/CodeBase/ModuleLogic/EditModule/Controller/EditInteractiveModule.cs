using System.Collections.Generic;
using UnityEngine;

public class EditInteractiveModule : EditModuleBase
{
    public override ModuleType ModuleType => ModuleType.Interactive;

    public override void EditModule(IModule module)
    {
        Debug.Log($"Edit {module.Name}");
    }
}
