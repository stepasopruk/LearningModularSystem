using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditModuleController : MonoBehaviour
{
    [SerializeField] private EditModuleView editModuleView;
    [SerializeField] private List<EditModuleBase> editModules;

    public void EditModule(IModule module)
    {
        EditModuleBase editModule = editModules.FirstOrDefault(x => x.ModuleType == module.ModuleType);
        
        if (editModule == null)
            return;

        editModuleView.gameObject.SetActive(true);
        editModuleView.RecordingEditList(editModule.DataModules);

        editModule.ModuleView.gameObject.SetActive(true);
        editModule.EditModule(module);
    }
}
