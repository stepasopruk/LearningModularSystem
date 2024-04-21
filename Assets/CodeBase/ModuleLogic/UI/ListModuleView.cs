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
        ToggleModuleView toggleModule = GetActiveToggleModuleView();

        if (toggleModule == null)
            return null;

        ToggleActive?.Invoke(false);
        toggleModule.Toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
        toggleGroup.UnregisterToggle(toggleModule.Toggle);
        _toggleModules.Remove(toggleModule);
        Destroy(toggleModule.gameObject);
        return toggleModule.Module;
    }

    public IModule EditModule()
    {
        ToggleModuleView toggleModule = GetActiveToggleModuleView();

        if (toggleModule == null)
            return null;

        return toggleModule.Module;
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

    private ToggleModuleView GetActiveToggleModuleView()
    {
        Toggle[] toggles = toggleGroup.ActiveToggles().ToArray();
        Toggle toggle = toggles.First();

        if (toggle.TryGetComponent(out ToggleModuleView toggleModule))
            return toggleModule;
        else
            return null;
    }
}
