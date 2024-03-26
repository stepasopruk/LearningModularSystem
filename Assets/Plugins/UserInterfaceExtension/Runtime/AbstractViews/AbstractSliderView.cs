using UnityEngine;
using UnityEngine.UI;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
namespace UserInterfaceExtension
{
    [RequireComponent(typeof(Slider))]
    public abstract class AbstractSliderView : MonoBehaviour
    {
        protected Slider _slider;

        protected void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        protected void OnDestroy() => 
            _slider.onValueChanged.RemoveListener(OnValueChanged);

        protected abstract void OnValueChanged(float value);
    }
}