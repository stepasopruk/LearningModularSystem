using System.Collections.Generic;
using UnityEngine;

public class EditModuleView : MonoBehaviour
{
    [SerializeField] private ListEditView listEditView;

    private void OnEnable()
    {
        listEditView.DestroyAllModuleView();
    }

    public void RecordingEditList(List<ModuleSO> dataModules)
    {
        foreach (ModuleSO module in dataModules)
            listEditView.CreateModuleView(module);
    }
}
