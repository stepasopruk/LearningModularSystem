using System;
using System.Collections.Generic;
using TestingSystem.Controllers;
using TestingSystem.Models;
using TestingSystem.Views;
using TestingSystem.Views.Answers;
using TestingSystem.Views.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestingSystem.Factories
{
    /// <summary>
    /// Class that creates views for answers.
    /// </summary>
    public class AnswerFactory
    {
        private readonly AnswersPrefabs _answersPrefabs;
        private readonly AnswerTextView _textAnswerViewPrefab;
        private readonly Dictionary<Answer, AnswerView> _answersViews;
        
        private Transform _container;
        private Action<Answer> _answerReceiveCallback;

        public AnswerFactory(AnswersPrefabs answersPrefabs)
        {
            _answersPrefabs = answersPrefabs;
            _answersViews = new Dictionary<Answer, AnswerView>();
        }
        
        /// <summary>
        /// Destroy all answers.
        /// </summary>
        public void ClearPreviousAnswers()
        {
            foreach (AnswerView answerView in _answersViews.Values) 
                Object.Destroy(answerView.gameObject);

            _answersViews.Clear();
        }

        /// <summary>
        /// Create text answer.
        /// </summary>
        /// <param name="answer">Answer to be spawned.</param>
        /// <param name="container">Container that stores answers.</param>
        /// <param name="answerHandler">Answer is dependency for answer view</param>
        public void CreateTextAnswer(TextAnswer answer, Transform container, AnswerHandler answerHandler)
        {
            AnswerTextView view = Spawn(_answersPrefabs.AnswerTextView, container);
            view.Construct(answerHandler);
            view.SetAnswer(answer);
            _answersViews.Add(answer, view);
        }
        
        private T Spawn<T>(T prefab, Transform container) where T : AnswerView => 
            Object.Instantiate(prefab, container);
    }
}