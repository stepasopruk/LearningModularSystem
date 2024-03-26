using TestingSystem.Controllers;
using TestingSystem.Models;
using UnityEngine;

namespace TestingSystem.Views
{
    /// <summary>
    /// Base implementation for answer views.
    /// </summary>
    public abstract class AnswerView : MonoBehaviour
    {
        private AnswerHandler _answerHandler;

        /// <summary>
        /// Method to initialize and add dependencies.
        /// Mandatory to call!
        /// </summary>
        /// <param name="answerHandler"></param>
        public void Construct(AnswerHandler answerHandler)
        {
            _answerHandler = answerHandler;
        }

        /// <summary>
        /// Toggles selection state.
        /// </summary>
        /// <param name="answer"></param>
        protected void AnswerSelected(Answer answer)
        {
            if (_answerHandler.IsSelected(answer))
                _answerHandler.RemoveAnswer(answer);
            else
                _answerHandler.AddAnswer(answer);
        }
    }
}