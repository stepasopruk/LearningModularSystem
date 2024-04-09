using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListModuleLessonView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private IcoModuleView icoModuleViewPrefab;

    private List<IcoModuleView> _icoModules;
    private List<IModule> _modules = new List<IModule>();

    public void Initialize(List<IModule> modules)
    {
        _icoModules = new List<IcoModuleView>();
        foreach (IModule module in modules)
        {
            if (_modules.Any(x => x == module))
                continue;

            IcoModuleView icoModuleView = Instantiate(icoModuleViewPrefab, content);
            icoModuleView.Sprite = module.Sprite;
            _modules.Add(module);
            _icoModules.Add(icoModuleView);
        }
    }
}
