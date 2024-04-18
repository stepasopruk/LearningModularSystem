

using UnityEngine;

public class TrainingInteractiveModule : IModule
{
    private const string _name = "Обучающий модуль";
    public ModuleType ModuleType => ModuleType.InteractiveTraining;
    public string Name => _name;
    public Sprite Sprite { get; set; }

}
