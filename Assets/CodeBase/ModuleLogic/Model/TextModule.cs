

using UnityEngine;

public class TextModule : IModule
{
    private const string _name = "������������� ������";
    public ModuleType ModuleType => ModuleType.Text;
    public string Name => _name;
    public Sprite Sprite { get; set; }

}
