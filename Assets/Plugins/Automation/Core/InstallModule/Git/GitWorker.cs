using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Automation.Git;
using Automation.Process;
using Automation.Runtime.Utils;

namespace Automation.Core.InstallModule.Git
{
    public class GitWorker
    {
        private readonly GitCallbackValidator _validator;
        private readonly IProcessStarter _processStarter;
        
        public GitWorker(IProcessStarter processStarter)
        {
            _validator = new GitCallbackValidator();
            _processStarter = processStarter;
        }
        
        public async Task<GitCallbackResult> GetTags(string repositoryUrl)
        {
            var tags = Array.Empty<string>();
            
            var tagRequestResult = await GetRepositoryTags(repositoryUrl);

            CustomDebug.Log(nameof(GitWorker), "Tag request callback " + tagRequestResult);

            if (string.IsNullOrWhiteSpace(tagRequestResult))
            {
                CustomDebug.Log(nameof(GitWorker), $"Tag request result: {GitCallback.CantReadRepository}");
                
                return new GitCallbackResult(GitCallback.CantReadRepository, tags);
            }

            var splitResult = tagRequestResult.Split('\n').Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
            
             tags = splitResult.Select(tagLine => tagLine.Trim().Split('/')[2]).ToArray();
             
             CustomDebug.Log(nameof(GitWorker), $"Tag request result: {GitCallback.Success}");
             
             return new GitCallbackResult(GitCallback.Success, tags);
        }

        public async Task<GitCallback> CloneRepositoryAsSubmodule(string repos, string folderTo)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {folderTo.PathToCommandLine()} submodule add -f {repos}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var processCallback = await _processStarter.Start(processInfo);
            
            var cloneResult = _validator.ValidateCloneResult(folderTo, processCallback, out var path);
            
            CustomDebug.Log(nameof(GitWorker),$"Clone repo as submodule callback: {processCallback}");
            CustomDebug.Log(nameof(GitWorker),$"Clone repo as submodule result: {cloneResult}");
            
            if (cloneResult == GitCallback.Success)
                await TrackRemoteRepository(path, repos);

            return cloneResult;
        }
        
        public async Task<GitCallback> CloneRepository(string repos, string folderTo)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {folderTo.PathToCommandLine()} clone {repos} . ",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var processCallback = await _processStarter.Start(processInfo);
            
            var cloneResult = _validator.ValidateCloneResult(folderTo, processCallback, out var path);

            CustomDebug.Log(nameof(GitWorker), $"Clone repo callback: {processCallback}");
            CustomDebug.Log(nameof(GitWorker), $"Clone repo result {cloneResult}");

                if (cloneResult == GitCallback.Success)
                await TrackRemoteRepository(path, repos);

            return cloneResult;
        }
        
        public async Task<GitCallback> CheckoutToTag(string repos, string tag)
        {
            var param = $"--detach tags/{tag}";

            var switchResult = await CheckoutTo(repos, param);
            
            var callback = _validator.IsFatalError(switchResult) ? GitCallback.TagDontExist : GitCallback.Success;

            CustomDebug.Log(nameof(GitWorker),$"Checkout to tag callback: {callback}");
            CustomDebug.Log(nameof(GitWorker),$"Checkout to tag result: {callback}");
            
            return callback;
        }

        public async Task<GitCallback> CheckoutToBranch(string repos, string branch, bool pullFromOrigin = false)
        {
            var remote = $"origin/{branch} --track origin/{branch}";
            var callback = await CheckoutTo(repos, remote);
            
            var isBranchExist = _validator.IsBranchExist(callback);

            if (isBranchExist) 
                callback = await CheckoutTo(repos, branch);

            var isError = _validator.IsFatalError(callback);

            var branchPos = isBranchExist ? "local" : "remote";
            var callbackRes = isError ? GitCallback.BranchDontExist : GitCallback.Success;
            
            CustomDebug.LogDeep(nameof(GitWorker),$"Checkout to {branchPos} branch callback: {callback}");
            CustomDebug.Log(nameof(GitWorker),$"Checkout to {branchPos} branch result: {callbackRes}");
            
            if (isError)
                return GitCallback.BranchDontExist;
            
            if (pullFromOrigin)
                await PullRepository(repos);

            return GitCallback.Success;
        }

        public async Task<GitCallbackResult> GetOriginUrl(string repos)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {repos.PathToCommandLine()} remote get-url origin",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            
            var parseResult = await _processStarter.Start(processInfo);

            if (_validator.IsFatalError(parseResult))
                return new GitCallbackResult(GitCallback.RepositoryNotFound, Array.Empty<string>());

            if (_validator.IsError(parseResult))
                return new GitCallbackResult(GitCallback.OriginNotFound, Array.Empty<string>());

            return new GitCallbackResult(GitCallback.Success, new string[]
            {
                parseResult
            });
        }

        public async Task<string> PullRepository(string repos)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {repos.PathToCommandLine()} pull",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            
            
            await StashAndFetch(repos);
            string callback = await _processStarter.Start(processInfo);
            
            CustomDebug.LogDeep(nameof(GitWorker),$"Pull repos callback: {callback}");
            return callback;
        }

        public async Task<string> RemoveSubmodule(string repos)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"submodule deinit -f {repos.PathToCommandLine()}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            
            string callback = await _processStarter.Start(processInfo);

            CustomDebug.LogDeep(nameof(GitWorker),$"submodule remove callback: {callback}, path at cmd {repos.PathToCommandLine()}");

            return callback;
        }

        private async Task<string> CheckoutTo(string repos, string toParam)
        {
            await StashAndFetch(repos);

            var checkoutProcess = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {repos.PathToCommandLine()} switch {toParam}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            return await _processStarter.Start(checkoutProcess);
        }

        private async Task<string> StashChanges(string repos)
        {
            var revertProcess = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {repos.PathToCommandLine()} stash",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            
            string callback = await _processStarter.Start(revertProcess);

            CustomDebug.LogDeep(nameof(GitWorker),$"stash changes callback: {callback}");

            return callback;
        }

        private async Task<string> GetRepositoryTags(string repos)
        {
            var getRemoteTagsProcess = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"ls-remote --tags {repos}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            
            return await _processStarter.Start(getRemoteTagsProcess);
        }

        private async Task<string> FetchAll(string repos)
        {
            var fetchProcess = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {repos.PathToCommandLine()} fetch --all --tags --prune",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            
            string callback = await _processStarter.Start(fetchProcess);
            
            CustomDebug.LogDeep(nameof(GitWorker),$"fetch callback: {callback}");
            return callback;
        }

        private async Task TrackRemoteRepository(string local, string origin)
        {
            var addOriginProcess = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"-C {local.PathToCommandLine()} remote add -tags -m master origin {origin}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            
            string callback = await _processStarter.Start(addOriginProcess);
            CustomDebug.LogDeep(nameof(GitWorker),$"track remote repos callback: {callback}");
        }

        private async Task<string> StashAndFetch(string repos) => 
            await StashChanges(repos) + await FetchAll(repos);
    }
}