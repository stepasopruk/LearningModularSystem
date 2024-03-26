using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace TestingSystem.Controllers
{
    /// <summary>
    /// Handler for selected answers in views;
    /// </summary>
    public class AnswerHandler : IDisposable
    {
        private readonly TestController _testController;
        
        private List<Answer> _answers;
        private Button _confirmButton;

        public AnswerHandler(TestController testController)
        {
            _answers = new List<Answer>();
            _testController = testController;
        }
        
        /// <summary>
        /// Follow button witch confirm answers
        /// </summary>
        /// <param name="button"></param>
        public void SetConfirmButton(Button button)
        {
            TryUnsubscribe();
            _confirmButton = button;
            _confirmButton.onClick.AddListener(Confirm);
        }
        
        /// <summary>
        /// Adds an answer to the list of selected answers
        /// </summary>
        /// <param name="answer">Answer to be added</param>
        public void AddAnswer(Answer answer)
        {
            if (!IsSelected(answer))
                _answers.Add(answer);
        }

        /// <summary>
        /// Removes an answer from the list of selected answers
        /// </summary>
        /// <param name="answer">Answer to be deleted</param>
        public void RemoveAnswer(Answer answer)
        {
            if (IsSelected(answer))
                _answers.Remove(answer);
        }
        
        /// <summary>
        /// Checks if this answer is selected
        /// </summary>
        /// <param name="answer"></param>
        /// <returns>Is selected</returns>
        public bool IsSelected(Answer answer) => 
            _answers.Contains(answer);

        public void Dispose() => TryUnsubscribe();

        private void TryUnsubscribe()
        {
            if (_confirmButton != null)
                _confirmButton.onClick.RemoveListener(Confirm);
        }

        private void Confirm()
        {
            if (!_answers.Any())
                return;
            
            _testController.ReceiveAnswer(_answers);
            _answers = new List<Answer>();
        }
    }
}