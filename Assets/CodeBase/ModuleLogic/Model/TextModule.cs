

using UnityEngine;

public class TextModule : IModule
{
    private const string _name = "Теоритический модуль";
    public ModuleType ModuleType => ModuleType.Text;
    public string Name => _name;
    public Sprite Sprite { get; set; }

}
