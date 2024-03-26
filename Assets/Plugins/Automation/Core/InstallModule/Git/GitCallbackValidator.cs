using System.IO;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Automation.Git
{
    public class GitCallbackValidator
    {
        private const int SPACE_OFFSET = 1;
        private const int CLONE_ERROR_INDEX = 0;
        private const int CLONE_COMMON_LINES_COUNT = 1;
        private const int CLONE_SUCCESS_LINES_COUNT = 2;
        private const int CLONE_CALLBACK_SPLIT_LENGTH = 3;
        private const int CLONE_CALLBACK_FOLDER_SPLIT_INDEX = 1;
        private const int CLONE_EXPECTED_REPOSITORY_ERROR_LINE_INDEX = 2;
        
        private const string CLONE_SUCCESS_CALLBACK = "done.";
        private const string CLONE_FAILURE_PATH_CALLBACK = "fatal: destination path ";
        private const string CLONE_FAILURE_CANT_READ_CALLBACK = "fatal: Could not read from remote repository.";
        
        private const string SWITCH_BRANCH_EXIST = "exists";
        private const string GIT_FATAL_ERROR = "fatal:";
        private const string GIT_ERROR = "error";

        public GitCallback ValidateCloneResult(string workFolder, string gitCloneCallback, out string repoFolderPath)
        {
            repoFolderPath = string.Empty;

            var callbackLines = gitCloneCallback.Split('\n').Where(l => !string.IsNullOrEmpty(l)).ToArray();

            if (IsCloneRepositoryError(callbackLines))
            {
                var cantReadRepositoryError = IsRepositoryUnavailable(callbackLines);

                return  cantReadRepositoryError ? GitCallback.CantReadRepository : GitCallback.UntrackedResponse;
            }

            var splitCallback = gitCloneCallback.Split('\'');

            if (splitCallback.Length != CLONE_CALLBACK_SPLIT_LENGTH)
                return GitCallback.UntrackedResponse;

            var folderName = splitCallback[CLONE_CALLBACK_FOLDER_SPLIT_INDEX];

            repoFolderPath = Path.Combine(workFolder, folderName);
            var hasDirectory = Directory.Exists(repoFolderPath);

            if (splitCallback[CLONE_ERROR_INDEX] == CLONE_FAILURE_PATH_CALLBACK && hasDirectory)
                return GitCallback.ClonePathExist;
            
            return IsCloneSuccess(gitCloneCallback) ? GitCallback.Success : GitCallback.UntrackedResponse;
        }

        private bool IsCloneSuccess(string cloneRes) => 
            ValidateCallbackLastSymbols(cloneRes, CLONE_SUCCESS_CALLBACK);

        public bool IsBranchExist(string checkoutRes) => 
            ValidateCallbackLastSymbols(checkoutRes, SWITCH_BRANCH_EXIST);

        public bool IsFatalError(string gitCallback) => 
            ValidateFirstSymbols(gitCallback, GIT_FATAL_ERROR);

        public bool IsError(string gitCallback) => 
            ValidateFirstSymbols(gitCallback, GIT_ERROR);
        
        private bool ValidateCallbackLastSymbols(string origin, string successResult)
        {
            var length = successResult.Length + SPACE_OFFSET;
#if UNITY_2021_3_OR_NEWER
            var substring = new string(origin[^length..].Where(c => !char.IsWhiteSpace(c)).ToArray());
#else
            var substring = new string(origin.Substring(0, length).Where(c => !char.IsWhiteSpace(c)).ToArray());
#endif
            return substring == successResult;
        }

        private bool ValidateFirstSymbols(string origin, string successResult)
        {
            var substring =
                new string(origin.Where(c => !char.IsWhiteSpace(c)).ToArray()).
                    Substring(0, successResult.Length);
            
            return substring == successResult;
        }
        
        private static bool IsCloneRepositoryError(string[] callbackLines) => 
            callbackLines.Length > CLONE_COMMON_LINES_COUNT &&
            callbackLines.Length != CLONE_SUCCESS_LINES_COUNT;

        private static bool IsRepositoryUnavailable(string[] callbackLines) =>
            callbackLines.Length > CLONE_EXPECTED_REPOSITORY_ERROR_LINE_INDEX &&
            callbackLines[CLONE_EXPECTED_REPOSITORY_ERROR_LINE_INDEX] == CLONE_FAILURE_CANT_READ_CALLBACK;
    }
}