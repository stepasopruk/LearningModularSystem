using UnityEngine;

public class EditVideoModule : EditModuleBase
{
    public override ModuleType ModuleType => ModuleType.Video;

    public override void EditModule(IModule module)
    {
        Debug.Log($"Edit {module.Name}");
    }
}