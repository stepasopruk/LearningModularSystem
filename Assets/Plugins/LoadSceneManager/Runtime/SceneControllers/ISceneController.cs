using System;

namespace LoadSceneManager.SceneControllers
{
    public interface ISceneController : IDisposable
    {
        public void LoadScene(string sceneName, float delay = 1, Action onComplete = null);
        
        public void LoadSceneAsync(string sceneName, float delay = 1, Action onComplete = null);
    }
}