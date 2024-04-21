using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Video.Views
{
    public sealed class TimelineView : MonoBehaviour, IPointerDownHandler
    {
        public event Action<float> OnTimelinePressed;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private RectTransform progressRect;

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null, out Vector2 localPoint);
            float selectedTime = localPoint.x;
            SetVideoProgressInternal(selectedTime);

            float selectedTimeNormalized = selectedTime / rectTransform.rect.width;
            OnTimelinePressed?.Invoke(selectedTimeNormalized);
        }

        public void SetVideoProgress(double time, double length)
        {
            time = time / length;
            float selectedTime = rectTransform.rect.width * (float)time;
            SetVideoProgressInternal(selectedTime);
        }

        private void SetVideoProgressInternal(float time) =>
            progressRect.sizeDelta = new Vector2(time, progressRect.sizeDelta.y);
    }
}
