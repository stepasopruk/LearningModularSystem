

using UnityEngine;

public class TestModule : IModule
{
    private const string _name = "Модуль проверки знаний";
    public ModuleType ModuleType => ModuleType.Test;
    public string Name => _name;
    public Sprite Sprite { get; set; }

    private TestModuleSO _moduleData;
    public ModuleSO ModuleData => _moduleData;

    public TestModule(TestModuleSO moduleData, Sprite sprite)
    {
        _moduleData = moduleData;
        Sprite = sprite;
        _moduleData.Module = this;
    }
}
