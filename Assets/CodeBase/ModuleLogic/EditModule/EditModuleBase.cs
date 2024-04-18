using UnityEngine;

public abstract class EditModuleBase : MonoBehaviour
{
    public abstract ModuleType ModuleType { get; }

    public abstract void EditModule(IModule module);
}
