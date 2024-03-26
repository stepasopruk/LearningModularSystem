#if UniTask
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace LoadSceneManager.SceneControllers
{
    public class UniTaskSceneController : BaseSceneController
    {
        private CancellationTokenSource _tokenSource;
        
        public UniTaskSceneController(ICoroutineRunner coroutineRunner) : base(coroutineRunner)
        {
        }
        
        public override async void LoadSceneAsync(string sceneName, float delay = 1, Action onComplete = null)
        {
            if (_tokenSource == null || !_tokenSource.Token.CanBeCanceled)
                _tokenSource = new CancellationTokenSource();
            
            SceneManager.LoadScene(LoadSceneName);

            await UniTask.RunOnThreadPool(() => LoadScene(sceneName, delay, _tokenSource.Token), true, _tokenSource.Token);
            onComplete?.Invoke();
        }
        
        protected override void ReleaseResources() => _tokenSource?.Dispose();

        private async UniTask LoadScene(string sceneName, float delay, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            await SceneManager.LoadSceneAsync(sceneName).WithCancellation(token);
        }
    }
}
#endif