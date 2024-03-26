using UnityEngine;
using UnityEngine.UI;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
namespace UserInterfaceExtension
{
    [RequireComponent(typeof(Toggle))]
    public abstract class AbstractToggleView : MonoBehaviour
    {
        protected Toggle _toggle;

        protected virtual void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnDestroy() => 
            _toggle.onValueChanged.RemoveListener(OnValueChanged);

        protected abstract void OnValueChanged(bool isOn);
    }
}