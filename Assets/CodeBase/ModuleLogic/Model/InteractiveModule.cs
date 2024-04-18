

using UnityEngine;

public class InteractiveModule : IModule
{
    private const string _name = "������������� ������";

    public ModuleType ModuleType => ModuleType.Interactive;
    public string Name => _name;
    public Sprite Sprite { get; set; }
}
