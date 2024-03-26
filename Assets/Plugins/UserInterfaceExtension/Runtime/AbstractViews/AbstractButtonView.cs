using UnityEngine;
using UnityEngine.UI;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
namespace UserInterfaceExtension
{
    [RequireComponent(typeof(Button))]
    public abstract class AbstractButtonView : MonoBehaviour
    {
        protected Button _button;

        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            
        }

        protected virtual void OnDestroy() => 
            _button.onClick.RemoveListener(OnClick);

        protected abstract void OnClick();
    }
}