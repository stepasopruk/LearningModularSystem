using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace CodeBase.Video.Library
{
    [CreateAssetMenu(fileName = "Video Library", menuName = "Libraries/Video Library")]
    public class VideoLibrary : ScriptableObject
    {
        [SerializeField]
        private List<VideoClip> videoClips;

        public List<VideoClip> VideoClips => videoClips;
    }
}
