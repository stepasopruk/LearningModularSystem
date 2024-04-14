using System;
using System.Collections.Generic;

public interface ILesson
{
    public event Action<IModule> ModelListChanged;

    public string Name { get; set; }

    public List<IModule> LessonModules { get; }

    void AddModule(IModule module);

    void RemoveModule(IModule module);
}