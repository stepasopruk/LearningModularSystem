

using UnityEngine;

public class TextModule : IModule
{
    private const string _name = "������������� ������";
    public string Name => _name;
    public Sprite Sprite { get; set; }

}
