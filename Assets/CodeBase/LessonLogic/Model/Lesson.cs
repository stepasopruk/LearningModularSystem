using System;
using System.Collections.Generic;

public class Lesson : ILesson
{
    public event Action<IModule> ModelListChanged;

    private string _name;
    public string Name 
    { 
        get => _name; 
        set => _name = value; 
    }

    private readonly List<IModule> _modules;
    public List<IModule> LessonModules => _modules;

    public Lesson()
    {
        _name = string.Empty;
        _modules = new List<IModule>();
    }

    public void AddModule(IModule module)
    {
        _modules.Add(module);
        ModelListChanged?.Invoke(module);
    }

    public void RemoveModule(IModule module)
    {
        _modules.Remove(module);
        ModelListChanged?.Invoke(module);
    }
}
