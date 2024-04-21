using System.Collections;
using CodeBase.Video.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Video.Controllers
{
    public sealed class VideoControlsView : MonoBehaviour, IPointerMoveHandler
    {
        private const float HIDE_VIDEO_CONTROLS_TIME = 3f;

        private Coroutine _hideControlButtonCoroutine;

        [SerializeField] private GameObject controlButtonsHolder;
        [SerializeField] private VideoController videoController;

        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button repeatButton;

        [Header("Control")]
        [SerializeField] private Slider volume;
        [SerializeField] private TimelineView timelineView;

        private void Awake()
        {
            videoController.VideoStarted += VideoController_OnVideoStarted;
            videoController.VideoPaused += VideoController_OnVideoPaused;
            videoController.VideoEnded += VideoController_OnVideoEnded;
            videoController.FrameChanged += VideoController_OnFrameChanged;

            playButton.onClick.AddListener(OnPlayVideoButtonClick);
            pauseButton.onClick.AddListener(OnPauseVideoButtonClick);
            repeatButton.onClick.AddListener(OnRepeatVideoButtonClick);

            volume.onValueChanged.AddListener(ChangeVolume);
            timelineView.OnTimelinePressed += TimelineView_OnTimelinePressed;
        }

        private void OnDestroy()
        {
            videoController.VideoStarted -= VideoController_OnVideoStarted;
            videoController.VideoPaused -= VideoController_OnVideoPaused;
            videoController.VideoEnded -= VideoController_OnVideoEnded;

            playButton.onClick.RemoveListener(OnPlayVideoButtonClick);
            pauseButton.onClick.RemoveListener(OnPauseVideoButtonClick);
            repeatButton.onClick.RemoveListener(OnRepeatVideoButtonClick);

            volume.onValueChanged.RemoveListener(ChangeVolume);
            timelineView.OnTimelinePressed -= TimelineView_OnTimelinePressed;
        }

        private void OnPlayVideoButtonClick()
        {
            videoController.PlayVideo();
        }

        private void OnPauseVideoButtonClick()
        {
            videoController.PauseVideo();
        }

        private void OnRepeatVideoButtonClick()
        {
            videoController.RepeatVideo();
        }

        /// <summary>
        /// Activates repeat button when video ends.
        /// </summary>
        private void VideoController_OnVideoEnded()
        {
            pauseButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            repeatButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// Activates play button when video paused.
        /// </summary>
        private void VideoController_OnVideoPaused()
        {
            pauseButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// Activates pause button when video started.
        /// </summary>
        private void VideoController_OnVideoStarted()
        {
            repeatButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// Changes timeline position depends on current video moment.
        /// </summary>
        /// <param name="videoTime"></param>
        /// <param name="videoLength"></param>
        private void VideoController_OnFrameChanged(double videoTime, double videoLength) =>
            timelineView.SetVideoProgress(videoTime, videoLength);

        /// <summary>
        /// Hides buttons if user didn't moved cursor over it for some time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator HideControlButtonRoutine()
        {
            yield return new WaitForSeconds(HIDE_VIDEO_CONTROLS_TIME);
            controlButtonsHolder.SetActive(false);
            _hideControlButtonCoroutine = null;
        }

        private void ChangeVolume(float value) =>
            videoController.ChangeVolume(value);

        /// <summary>
        /// Activates interface if cursor moved over video.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerMove(PointerEventData eventData)
        {
            controlButtonsHolder.SetActive(true);
            if (_hideControlButtonCoroutine != null)
                StopCoroutine(_hideControlButtonCoroutine);

            _hideControlButtonCoroutine = StartCoroutine(HideControlButtonRoutine());
        }

        /// <summary>
        /// Changes current frame based on timeline click.
        /// </summary>
        /// <param name="selectedTimeNormalized"></param>
        private void TimelineView_OnTimelinePressed(float selectedTimeNormalized)
        {
            if (repeatButton.gameObject.activeSelf)
            {
                repeatButton.gameObject.SetActive(false);
                OnPlayVideoButtonClick();
            }

            videoController.SetVideoTime(selectedTimeNormalized);
        }
    }
}
