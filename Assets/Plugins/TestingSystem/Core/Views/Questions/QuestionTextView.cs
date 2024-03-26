using UnityEngine;
using UnityEngine.UI;

namespace TestingSystem.Views.Questions
{
    /// <summary>
    /// View for text questions.
    /// </summary>
    public class QuestionTextView : QuestionView
    {
        [SerializeField] private Text view;
        
        /// <summary>
        /// Display question in view
        /// </summary>
        /// <param name="question">question to display</param>
        public void SetQuestion(string question) => view.text = question;
    }
}