using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ModulesListView : MonoBehaviour
{
    public event Action<bool> ToggleActive;

    [SerializeField] private Transform content;
    [SerializeField] private ToggleModuleView toggleModulePrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<ToggleModuleView> _toggleModules;
    private List<Toggle> _togglesView;

    private void Awake()
    {
        _toggleModules = new List<ToggleModuleView>();
        _togglesView = new List<Toggle>();
    }

    public void AddModuleList(IModule module)
    {
        ToggleModuleView toggleModuleView = Instantiate(toggleModulePrefab, content);
        toggleModuleView.Initialize(module, toggleGroup);
        _toggleModules.Add(toggleModuleView);

        if (toggleModuleView.TryGetComponent(out Toggle toggle))
        {
            toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
            _togglesView.Add(toggle);
        }
    }

    private void Toggle_OnValueChanged(bool active)
    {
        if(active)
            ToggleActive?.Invoke(true);
        else if(!toggleGroup.AnyTogglesOn())
            ToggleActive?.Invoke(false);
    }

    public IModule RemoveModuleList() 
    {
        Toggle[] toggles = toggleGroup.ActiveToggles().ToArray();

        foreach (Toggle toggle in toggles)
        {
            if (!toggle.isOn)
                continue;

            if(toggle.TryGetComponent(out ToggleModuleView toggleModule))
            {
                ToggleActive?.Invoke(false);
                toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
                toggleGroup.UnregisterToggle(toggle);
                _toggleModules.Remove(toggleModule);
                _togglesView.Remove(toggle);
                Destroy(toggleModule.gameObject);
                return toggleModule.Module;
            }
        }

        return null;
    }

    private void OnDestroy()
    {
        foreach(Toggle toggle in _togglesView)
            toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
    }
}
