public interface ILesson
{
    public string Name { get; set; }

    void AddModule(IModule module);

    void RemoveModule(IModule module);
}