using UnityEngine;

public interface IModule
{
    ModuleType ModuleType { get; }

    string Name { get; }

    Sprite Sprite { get; set; }
}

public enum ModuleType
{
    None = 0,
    Text = 1,
    Interactive = 2,
    InteractiveTraining = 3,
    Video = 4,
    Test = 5
}
