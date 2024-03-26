// ReSharper disable once CheckNamespace

namespace Automation.Git
{
    [System.Serializable]
    public class RawTabCollection
    {
        public RawTab[] Categories;
    }

    [System.Serializable]
    public class RawTab
    {
        public string Name;
        public GitInfo[] Items;
    }
}