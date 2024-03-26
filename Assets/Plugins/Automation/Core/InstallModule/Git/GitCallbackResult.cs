
// ReSharper disable once CheckNamespace
namespace Automation.Git
{
    public class GitCallbackResult
    {
        public readonly GitCallback GitCallback;
        public readonly string[] Arguments;

        public GitCallbackResult(GitCallback gitCallback, string[] arguments)
        {
            GitCallback = gitCallback;
            Arguments = arguments;
        }
    }
}