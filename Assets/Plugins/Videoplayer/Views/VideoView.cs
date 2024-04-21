using CodeBase.Utilities.Searching;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UserInterfaceExtension;

namespace CodeBase.Video.Views
{
    public sealed class VideoView : AbstractButtonView, ISearchable
    {
        [SerializeField]
        private TextMeshProUGUI view;

        private VideoClip _videoClip;
        private VideoMenuView _videoMenuView;

        public string Text => view.text;

        public void Construct(VideoClip videoClip, VideoMenuView videoMenuView)
        {
            _videoClip = videoClip;
            _videoMenuView = videoMenuView;
            view.text = _videoClip.name;
        }

        protected override void OnClick() =>
            _videoMenuView.OpenNewVideo(_videoClip);
    }
}
