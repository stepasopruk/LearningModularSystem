using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    [SerializeField] private LessonController lessonController;
    [SerializeField] private ModulesListView modulesListView;

    private List<IModule> _modules;

    private void Awake()
    {
        _modules = new List<IModule>();
    }

    public void AddModuleLesson(IModule module)
    {
        _modules.Add(module);
        lessonController.CurrentLesson.AddModule(module);
        modulesListView.AddModuleList(module);
    }
}
