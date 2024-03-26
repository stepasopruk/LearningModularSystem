using System;
using UnityEngine;
using UnityEngine.UI;

namespace LoadSceneManager.Utils
{
    public class MessageBox : MonoBehaviour
    {
        private const string PREFAB_NAME = "LSM_MessageBox";
        private const string NOTIFY = "Оповещение!";
        private const string ERROR = "Ошибка!";
        
        private static MessageBox s_instance;

        [SerializeField] private GameObject placeholder;
        [SerializeField] private Text titleView;
        [SerializeField] private Text messageView;
        [SerializeField] private Button confirmButton;
        
        private Action _clickAction; 
        
        
        public static void Display(MessageType type, string message, Action onClick = null)
        {
            if (s_instance == null)
            {
                s_instance = Instantiate(Resources.Load<MessageBox>(PREFAB_NAME));
                DontDestroyOnLoad(s_instance);
            }
            
            s_instance.ConfigureMessage(type, message, onClick);
            s_instance.gameObject.SetActive(true);
        }

        private void OnEnable() => 
            confirmButton.onClick.AddListener(ConfirmButton_OnClick);

        private void OnDisable() => 
            confirmButton.onClick.RemoveListener(ConfirmButton_OnClick);

        private void ConfigureMessage(MessageType type, string message, Action onClick = null)
        {
            titleView.text = GetTitle(type);
            messageView.text = message;
            _clickAction = onClick;
            placeholder.SetActive(true);
        }
        
        private void ConfirmButton_OnClick()
        {
            _clickAction?.Invoke();
            placeholder.SetActive(false);
        }
        
        private string GetTitle(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    return ERROR;
                case MessageType.Notify:
                    return NOTIFY;
                default:
                    return string.Empty;
            }
        }
    }
}