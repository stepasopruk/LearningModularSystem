using UnityEngine;

public class EditTextModule : EditModuleBase
{
    public override ModuleType ModuleType => ModuleType.Text;

    public override void EditModule(IModule module)
    {
        Debug.Log($"Edit {module.Name}");
    }
}
