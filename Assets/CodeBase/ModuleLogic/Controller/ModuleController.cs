using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    public event Action<bool> ToggleActive;

    [SerializeField] private LessonController lessonController;
    [SerializeField] private ListModuleView modulesListView;

    private List<IModule> _modules;

    private void Awake()
    {
        _modules = new List<IModule>();
        modulesListView.ToggleActive += ModulesListView_ToggleActive;
    }

    private void OnDestroy() =>
        modulesListView.ToggleActive -= ModulesListView_ToggleActive;

    public void AddModuleLesson(IModule module)
    {
        _modules.Add(module);
        lessonController.CurrentLesson.AddModule(module);
        modulesListView.AddModuleList(module);
    }

    public void RemoveModuleLesson()
    {
        IModule module = modulesListView.RemoveModule();

        if (module == null)
            return;

        _modules.Remove(module);
        lessonController.CurrentLesson.RemoveModule(module);
    }

    private void ModulesListView_ToggleActive(bool active) => 
        ToggleActive?.Invoke(active);
}
