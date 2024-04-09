using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ListModuleView : MonoBehaviour
{
    public event Action<bool> ToggleActive;

    [SerializeField] private Transform content;
    [SerializeField] private ToggleModuleView toggleModulePrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private List<ToggleModuleView> _toggleModules;

    private void Awake() => 
        _toggleModules = new List<ToggleModuleView>();

    private void OnDisable() =>
        RemoveAllModule();

    public void AddModuleList(IModule module)
    {
        ToggleModuleView toggleModuleView = Instantiate(toggleModulePrefab, content);
        toggleModuleView.Initialize(module, toggleGroup);
        toggleModuleView.Toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
        _toggleModules.Add(toggleModuleView);
    }

    private void Toggle_OnValueChanged(bool active)
    {
        if (active)
            ToggleActive?.Invoke(true);
        else if (!toggleGroup.AnyTogglesOn())
            ToggleActive?.Invoke(false);
    }

    public IModule RemoveModule()
    {
        Toggle[] toggles = toggleGroup.ActiveToggles().ToArray();

        foreach (Toggle toggle in toggles)
        {
            if (!toggle.isOn)
                continue;

            if (toggle.TryGetComponent(out ToggleModuleView toggleModule))
            {
                ToggleActive?.Invoke(false);
                toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
                toggleGroup.UnregisterToggle(toggle);
                _toggleModules.Remove(toggleModule);
                Destroy(toggleModule.gameObject);
                return toggleModule.Module;
            }
        }

        return null;
    }

    private void RemoveAllModule()
    {
        foreach (ToggleModuleView toggleModuleView in _toggleModules)
        {
            toggleModuleView.Toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
            Destroy(toggleModuleView.gameObject);
        }
        _toggleModules = new List<ToggleModuleView>();
    }
}
