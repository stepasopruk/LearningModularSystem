

using UnityEngine;

public class VideoModule : IModule
{
    private const string _name = "Видеомодуль";
    public ModuleType ModuleType => ModuleType.Video;
    public string Name => _name;
    public Sprite Sprite { get; set; }

    private VideoModuleSO _moduleData;
    public ModuleSO ModuleData => _moduleData;

    public VideoModule(VideoModuleSO moduleData, Sprite sprite)
    {
        _moduleData = moduleData;
        Sprite = sprite;
        _moduleData.Module = this;
    }
}
