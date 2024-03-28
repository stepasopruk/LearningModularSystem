using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleModuleView : MonoBehaviour
{   
    [SerializeField] private Text title;

    private Toggle toggle;

    private IModule _module;
    public IModule Module => _module;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void Initialize(IModule module, ToggleGroup group)
    {
        _module = module;
        title.text = module.Name;
        toggle.group = group;
    }
}
