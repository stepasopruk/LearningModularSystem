

using UnityEngine;

public class InteractiveModule : IModule
{
    private const string _name = "Интерактивный модуль";

    public ModuleType ModuleType => ModuleType.Interactive;
    public string Name => _name;
    public Sprite Sprite { get; set; }

    private InteractiveModuleSO _moduleData;
    public ModuleSO ModuleData => _moduleData;

    public InteractiveModule(InteractiveModuleSO moduleData, Sprite sprite)
    {
        _moduleData = moduleData;
        Sprite = sprite;
        _moduleData.Module = this;
    }
}
