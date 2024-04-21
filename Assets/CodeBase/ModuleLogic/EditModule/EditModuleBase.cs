using System.Collections.Generic;
using UnityEngine;

public abstract class EditModuleBase : MonoBehaviour
{
    [SerializeField] protected ModuleViewBase moduleView;
    [SerializeField] protected List<ModuleSO> moduleViewSO;

    public ModuleViewBase ModuleView => moduleView;
    public List<ModuleSO> DataModules => moduleViewSO;

    public abstract ModuleType ModuleType { get; }

    public abstract void EditModule(IModule module);
}
