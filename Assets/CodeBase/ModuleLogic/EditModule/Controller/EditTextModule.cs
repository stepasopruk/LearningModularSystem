using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditTextModule : EditModuleBase
{
    public override ModuleType ModuleType => ModuleType.Text;

    public override void EditModule(IModule module)
    {
        Debug.Log($"Edit {module.Name}");
        ModuleSO moduleSO = moduleViewSO.FirstOrDefault(x => x.Module == module);

        if (moduleSO == null)
            CreateModuleData(module, out moduleSO);

        moduleView.gameObject.SetActive(true);
        moduleView.RecordingDataView(moduleSO);
    }

    private void CreateModuleData(IModule module, out ModuleSO moduleSO)
    {
        moduleSO = ScriptableObject.CreateInstance<TextModuleSO>();
        moduleSO.Module = module;
        AssetDatabase.CreateAsset(moduleSO, "Assets/TextModule.asset");
    }
}
