

using UnityEngine;

public class TestModule : IModule
{
    private const string _name = "������ �������� ������";
    public ModuleType ModuleType => ModuleType.Test;
    public string Name => _name;
    public Sprite Sprite { get; set; }
}
