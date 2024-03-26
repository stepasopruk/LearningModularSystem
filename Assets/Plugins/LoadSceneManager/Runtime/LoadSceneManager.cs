using System;
using LoadSceneManager.Conditions.Core;
using LoadSceneManager.Data;
using LoadSceneManager.SceneControllers;
using LoadSceneManager.Utils;
using UnityEngine;
#if UniTask
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace LoadSceneManager
{
    public class LoadSceneManager : MonoBehaviour, ICoroutineRunner
    {
        private static LoadSceneManager s_Instance;
        
        [Header("Async Loading"), SerializeField] private bool loadAsync;
        
        [SerializeField] private AbstractLoadCondition[] conditions;

        [Header("Сцена которая загружается после Bootstrap")]
        [SerializeField, HideInInspector] private SceneInfo _nextScene;
        
        [Header("Задрежка при первой загрузке.")]
        [SerializeField, Range(0, 10)] private float delayBeforeFirstLoading = 3;
        [Header("Задержка в загруке")]
        [SerializeField, Range(0, 10)] private float delayInLoading = 3;

        private ISceneController _sceneController;
        
        private void Awake()
        {
            if (s_Instance != null)
            {
                Destroy(this);
                return;
            }

            s_Instance = this;
            DontDestroyOnLoad(s_Instance);
            
            _sceneController = CreateSceneController(this);

            if (conditions.Length > 0)
            {
                foreach (AbstractLoadCondition condition in conditions) 
                    condition.Initialize();
            }

            CheckConditions(() => LoadScene(_nextScene.Name, delayBeforeFirstLoading));
        }

        public static void LoadScene(string sceneName) => s_Instance.LoadScene(sceneName, s_Instance.delayInLoading);

        private void LoadScene(string sceneName, float delay)
        {
            if (loadAsync)
                _sceneController.LoadSceneAsync(sceneName, delay);
            else
                _sceneController.LoadScene(sceneName, delay);
        }
        private ISceneController CreateSceneController(ICoroutineRunner coroutineRunner)
        {
#if UniTask
            return new UniTaskSceneController(coroutineRunner);
#else
            return new SceneController(coroutineRunner);
#endif
        }
        
        private async void CheckConditions(Action conditionPerformed)
        {
            var condition = await CheckConditions();

            if (condition != null) 
                MessageBox.Display(MessageType.Error, condition.ErrorMessage, Application.Quit);
            else
                conditionPerformed?.Invoke();
        }
        
#if UniTask
        private async UniTask<AbstractLoadCondition> CheckConditions()
        {
            foreach (AbstractLoadCondition condition in conditions)
            { 
                await CheckCondition(condition);

                if (condition.Status == ConditionState.Error && condition.Mandatory)
                    return condition;
            }
            
            return null;
        }

        private async UniTask<bool> CheckCondition(AbstractLoadCondition condition)
        {
            while (condition.Status == ConditionState.Loading) 
                await UniTask.Yield();

            switch (condition.Status)
            {
                case ConditionState.Error:
                    return false;
                case ConditionState.Success:
                    return true;
            }

            return false;
        }
#else
        private async Task<AbstractLoadCondition> CheckConditions()
        {
            foreach (AbstractLoadCondition condition in conditions)
            {
               await CheckCondition(condition);

               if (condition.Status == ConditionState.Error && condition.Mandatory)
                   return condition;
            }

            return null;
        }
        
        private async Task CheckCondition(AbstractLoadCondition condition)
        {
            while (condition.Status == ConditionState.Loading) 
                await Task.Yield();
        }
#endif
    }
}