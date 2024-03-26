using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadSceneManager.SceneControllers
{
    public class SceneController : BaseSceneController
    {
        private Coroutine _coroutine;
        private CancellationTokenSource _tokenSource;

        public SceneController(ICoroutineRunner coroutineRunner) : base(coroutineRunner)
        {
        }

        public override async void LoadSceneAsync(string sceneName, float delay = 1, Action onComplete = null)
        {
            if (_tokenSource == null || _tokenSource.IsCancellationRequested)
                _tokenSource = new CancellationTokenSource();
            
            await LoadSceneAsync(sceneName, delay, _tokenSource.Token);
            onComplete?.Invoke();
        }
        
        private async Task LoadSceneAsync(string scene, float delay, CancellationToken token)
        {
            SceneManager.LoadScene(LoadSceneName);
            
            await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            AsyncOperation op = SceneManager.LoadSceneAsync(scene);
            
            while (!op.isDone && !token.IsCancellationRequested) 
                await Task.Yield();
        }

        protected override void ReleaseResources() => _tokenSource?.Dispose();
    }
}
