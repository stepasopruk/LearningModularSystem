// ReSharper disable once CheckNamespace
namespace Automation.Git
{
    public enum GitCallback
    {
        Success = 0,
        ClonePathExist = 1,
        CantReadRepository = 2,
        UntrackedResponse = 3,
        BranchDontExist = 4,
        TagDontExist = 5,
        OriginNotFound = 6,
        RepositoryNotFound = 7,
    }
}