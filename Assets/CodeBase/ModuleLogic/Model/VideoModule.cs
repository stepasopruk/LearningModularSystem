

using UnityEngine;

public class VideoModule : IModule
{
    private const string _name = "Видеомодуль";
    public string Name => _name;

    public Sprite Sprite { get; set; }
}
