

using UnityEngine;

public class VideoModule : IModule
{
    private const string _name = "Видеомодуль";
    public ModuleType ModuleType => ModuleType.Video;
    public string Name => _name;
    public Sprite Sprite { get; set; }
}
