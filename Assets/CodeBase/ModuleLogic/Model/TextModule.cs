

using UnityEngine;

public class TextModule : IModule
{
    private const string _name = "Теоритический модуль";
    public ModuleType ModuleType => ModuleType.Text;
    public string Name => _name;
    public Sprite Sprite { get; set; }

    private TextModuleSO _moduleData;
    public ModuleSO ModuleData => _moduleData;

    public TextModule(TextModuleSO moduleData, Sprite sprite)
    {
        _moduleData = moduleData;
        Sprite = sprite;
        _moduleData.Module = this;
    }
}
