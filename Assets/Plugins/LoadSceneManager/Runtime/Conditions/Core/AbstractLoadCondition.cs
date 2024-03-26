using UnityEngine;

namespace LoadSceneManager.Conditions.Core
{
    public abstract class AbstractLoadCondition : ScriptableObject
    {
        public abstract ConditionState Status { get; }

        public bool Mandatory => !_disableInEditor && mandatory;
        
        public string ErrorMessage => _errorMessage;
        
        [Header("Обязательное условие. В случае ошибки появляется сообщение.")]
        [SerializeField] private bool mandatory;
#if UNITY_EDITOR
        [Header("Editor only: делает условие не обязательным.")]
        [SerializeField] private bool _disableInEditor;
#else
        private bool _disableInEditor = false;
#endif
        [Header("Сообщение об ошибке.")]
        [TextArea, SerializeField] protected string _errorMessage;

        public abstract void Initialize();
    }

    /// <summary>
    /// Класс для инициализации в ручную.
    /// </summary>
    public class SelfInitializedCondition : AbstractLoadCondition
    {
        public override ConditionState Status => _status;

        private ConditionState _status = ConditionState.Loading;
        
        public override void Initialize()
        {
            if (_status != ConditionState.Sleep)
                _status = ConditionState.Loading;
        }

        /// <summary>
        /// Установить статус загрузки.
        /// </summary>
        /// <param name="state"></param>
        public void SetStatus(ConditionState state) => _status = state;
    }
}