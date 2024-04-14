using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListModuleLessonView : MonoBehaviour
{
    private class IcoModules
    {
        public IModule Module;
        public IcoModuleView IcoModuleView;
    }

    [SerializeField] private Transform content;
    [SerializeField] private IcoModuleView icoModuleViewPrefab;

    private List<IcoModules> _icoModules = new List<IcoModules>();

    internal void UpdateListModules(IModule module)
    {
        IcoModules icoModules = _icoModules.FirstOrDefault(x => x.Module == module);

        if (icoModules == null)
        {
            IcoModuleView icoModuleView = Instantiate(icoModuleViewPrefab, content);
            icoModules = new IcoModules();
            icoModules.Module = module;
            icoModules.IcoModuleView = icoModuleView;
            icoModuleView.Sprite = module.Sprite;
            _icoModules.Add(icoModules);
        }
        else
        {
            Destroy(icoModules.IcoModuleView.gameObject);
        }
    }
}
