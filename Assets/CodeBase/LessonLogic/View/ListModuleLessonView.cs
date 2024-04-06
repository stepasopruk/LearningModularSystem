using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListModuleLessonView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private IcoModuleView icoModuleViewPrefab;

    private List<IcoModuleView> _icoModules;

    public void Initialize(List<IModule> modules)
    {
        _icoModules = new List<IcoModuleView>();
        foreach (IModule module in modules)
        {
            IcoModuleView icoModuleView = Instantiate(icoModuleViewPrefab, content);
            icoModuleView.Sprite = module.Sprite;
            _icoModules.Add(icoModuleView);
        }
    }
}
