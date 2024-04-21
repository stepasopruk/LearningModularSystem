using System.Collections.Generic;
using System.Linq;
using CodeBase.Utilities.Searching;
using CodeBase.Video.Controllers;
using CodeBase.Video.Library;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace CodeBase.Video.Views
{
    public sealed class VideoMenuView : MonoBehaviour
    {
        [Header("Modules List")]
        [SerializeField] private TMP_InputField videoSearchField;
        [SerializeField] private VideoLibrary videosLibrary;
        [SerializeField] private VideoView videoViewPrefab;
        [SerializeField] private Transform container;

        [Space]
        [SerializeField] private VideoController videoController;

        private readonly List<VideoView> _videoViews = new();

        private void Awake()
        {
            videoSearchField.onValueChanged.AddListener(OnSearchTextChanged);
            LoadModules();
        }

        private void OnDestroy() =>
            videoSearchField.onValueChanged.RemoveListener(OnSearchTextChanged);

        private void LoadModules()
        {
            foreach (VideoClip videoClip in videosLibrary.VideoClips)
            {
                VideoView videoView = Instantiate(videoViewPrefab, container);
                videoView.Construct(videoClip, this);
                _videoViews.Add(videoView);
            }
        }

        public void OpenNewVideo(VideoClip videoClip)
        {
            videoController.gameObject.SetActive(true);
            videoController.SetVideoClip(videoClip);
        }

        private void OnSearchTextChanged(string text)
        {
            List<ISearchable> videos = _videoViews.GetElementsContain(text).ToList();
            foreach (VideoView video in _videoViews)
            {
                bool isActive = videos.Contains(video);
                video.gameObject.SetActive(isActive);
            }
        }
    }
}

