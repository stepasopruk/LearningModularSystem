using TestingSystem.Views.Answers;
using UnityEngine;

namespace TestingSystem.Views.Data
{
    /// <summary>
    /// Storage for view prefabs.
    /// </summary>
    [CreateAssetMenu(menuName = "Testing System/Create AnswersPrefabs", fileName = "AnswersPrefabs")]
    public class AnswersPrefabs : ScriptableObject
    {
        /// <summary>
        /// Text view prefab.
        /// </summary>
        public AnswerTextView AnswerTextView => answerTextView;
        
        [SerializeField] private AnswerTextView answerTextView;
    }
}