using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadSceneManager.SceneControllers
{
    public abstract class BaseSceneController : ISceneController
    {
        protected const string LoadSceneName = "Loading";
        private readonly ICoroutineRunner _coroutineRunner;
        
        private Coroutine _coroutine;

        protected BaseSceneController(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public void LoadScene(string sceneName, float delay = 1, Action onComplete = null)
        {
            SceneManager.LoadScene(LoadSceneName);
            _coroutine = _coroutineRunner.StartCoroutine(LoadSceneCoroutine(sceneName, onComplete, delay));
        }

        public abstract void LoadSceneAsync(string sceneName, float delay = 1, Action onComplete = null);

        protected abstract void ReleaseResources();
        
        public void Dispose()
        {
            if (_coroutine != null)
                _coroutineRunner.StopCoroutine(_coroutine);
            
            ReleaseResources();
        }
        
        private IEnumerator LoadSceneCoroutine(string sceneName, Action onComplete = null, float delay = 1)
        {
            yield return new WaitForSecondsRealtime(delay);
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            
            yield return new WaitUntil(() => op.isDone);
            onComplete?.Invoke();

            _coroutine = null;
        }
    }
}