using System.Collections.Generic;
using UnityEngine;

public class ListEditView : MonoBehaviour
{
    [SerializeField] private ModuleViewItem moduleViewItemPrefab;
    [SerializeField] private Transform content;

    private List<ModuleViewItem> moduleViews = new List<ModuleViewItem>();

    public void CreateModuleView(ModuleSO module)
    {
        ModuleViewItem moduleViewItem = Instantiate(moduleViewItemPrefab, content);
        moduleViewItem.Initialize(module.ModuleName);
        //moduleViewItem.Button.onClick.AddListener(delegate {  })
        moduleViews.Add(moduleViewItem);
    }

    public void DestroyAllModuleView()
    {
        foreach (ModuleViewItem moduleViewItem in moduleViews)
        {
            Destroy(moduleViewItem);
        }
        moduleViews = new List<ModuleViewItem>();
    }


}
