using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleModuleView : MonoBehaviour
{   
    [SerializeField] private Text title;

    private Toggle _toggle;
    public Toggle Toggle => _toggle;

    private IModule _module;
    public IModule Module => _module;

    public void Initialize(IModule module, ToggleGroup group)
    {
        _toggle = GetComponent<Toggle>();
        _module = module;
        title.text = module.Name;
        _toggle.group = group;
    }
}
