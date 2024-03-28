using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModulesListView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private ToggleModuleView toggleModulePrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<ToggleModuleView> toggleModules;

    private void Awake()
    {
        toggleModules = new List<ToggleModuleView>();
    }

    public void AddModuleList(IModule module)
    {
        ToggleModuleView toggleModuleView = Instantiate(toggleModulePrefab, content);
        toggleModuleView.Initialize(module, toggleGroup);
        toggleModules.Add(toggleModuleView);
    }

    public void RemoveModuleList(IModule module) 
    {
        ToggleModuleView toggleModule = toggleModules.Find(x => x.Module == module);
        toggleModules.Remove(toggleModule);
        Destroy(toggleModule);
    }  
}
