using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CodeBase.Video.Controllers
{
    public sealed class VideoController : MonoBehaviour
    {
        public event Action VideoStarted;
        public event Action VideoPaused;
        public event Action VideoEnded;

        public event Action<double, double> FrameChanged;

        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RawImage videoPlayerImage;

        public VideoPlayer VideoPlayer => videoPlayer;

        private long _lastFrame = 0;

        private void Awake()
        {
            videoPlayer.targetTexture = new RenderTexture(
                (int)videoPlayerImage.rectTransform.sizeDelta.x,
                (int)videoPlayerImage.rectTransform.sizeDelta.y,
                videoPlayerImage.depth);

            videoPlayerImage.texture = videoPlayer.targetTexture;
            videoPlayer.loopPointReached += VideoPlayer_OnLoopPointReached;
        }

        private void OnDestroy()
        {
            videoPlayer.loopPointReached -= VideoPlayer_OnLoopPointReached;
        }

        private void OnEnable()
        {
            videoPlayer.Prepare();
            videoPlayer.frame = _lastFrame;
        }

        private void OnDisable() =>
            PauseVideo();

        private void Update()
        {
            if (videoPlayer.isPlaying && _lastFrame != videoPlayer.frame)
            {
                _lastFrame = videoPlayer.frame;
                FrameChanged?.Invoke(videoPlayer.time, videoPlayer.length);
            }
        }

        /// <summary>
        /// If video ends repeat button activates.
        /// </summary>
        /// <param name="source"></param>
        private void VideoPlayer_OnLoopPointReached(VideoPlayer source) =>
            VideoEnded?.Invoke();

        /// <summary>
        /// Sets current time of video.
        /// </summary>
        /// <param name="timeNormalized">Normalized value of time</param>
        public void SetVideoTime(double timeNormalized) =>
            videoPlayer.time = videoPlayer.length * timeNormalized;

        /// <summary>
        /// Set current video.
        /// </summary>
        /// <param name="videoClip"></param>
        public void SetVideoClip(VideoClip videoClip)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.Prepare();
            PauseVideo();
        }

        /// <summary>
        /// Start playing video.
        /// </summary>
        public void PlayVideo() =>
            StartCoroutine(PlayVideoInternal());

        private IEnumerator PlayVideoInternal()
        {
            while (!videoPlayer.isPrepared)
                yield return null;

            videoPlayer.Play();
            VideoStarted?.Invoke();
        }

        /// <summary>
        /// Stops video.
        /// </summary>
        public void PauseVideo()
        {
            videoPlayer.Pause();
            VideoPaused?.Invoke();
        }

        /// <summary>
        /// Play video from the beggining.
        /// </summary>
        public void RepeatVideo()
        {
            videoPlayer.frame = 0;
            PlayVideo();
        }

        /// <summary>
        /// Changes volume of the video.
        /// </summary>
        /// <param name="value"></param>
        public void ChangeVolume(float value) =>
            videoPlayer.SetDirectAudioVolume(0, value);
    }
}
