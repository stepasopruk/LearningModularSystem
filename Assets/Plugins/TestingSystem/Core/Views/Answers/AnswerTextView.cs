using System;
using TestingSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace TestingSystem.Views.Answers
{
    /// <summary>
    /// View for text answers.
    /// </summary>
    public class AnswerTextView : AnswerView
    {
        [SerializeField] private Button button;
        [SerializeField] private Text view;

        private TextAnswer _answer;

        private void Awake() => 
            button.onClick.AddListener(AnswerSelected);

        private void OnDestroy() => 
            button.onClick.RemoveListener(AnswerSelected);

        private void AnswerSelected()
        {
#if UNITY_EDITOR
            if (_answer == null)
                throw new NullReferenceException($"{nameof(_answer)} is null");
#endif
            AnswerSelected(_answer);
        }

        /// <summary>
        /// Links answer for this view.
        /// </summary>
        /// <param name="answer">Answer to be linked.</param>
        public void SetAnswer(TextAnswer answer)
        {
            _answer = answer;
            view.text = _answer.Text;
        }
    }
}