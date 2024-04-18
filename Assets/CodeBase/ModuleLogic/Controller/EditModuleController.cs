using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditModuleController : MonoBehaviour
{
    [SerializeField] private List<EditModuleBase> editModules;

    public void EditModule(IModule module)
    {
        EditModuleBase editModule = editModules.FirstOrDefault(x => x.ModuleType == module.ModuleType);
        
        if (editModule == null)
            return;

        editModule.EditModule(module);
    }
}
