using System.Collections.Generic;
using TestingSystem.Factories;
using TestingSystem.Models;
using TestingSystem.Views;
using TestingSystem.Views.Answers;
using TestingSystem.Views.Questions;
using UnityEngine;

namespace TestingSystem.Controllers
{
    /// <summary>
    /// Controller for views
    /// </summary>
    public class QuestionInspector
    {
        private readonly QuestionFactory _questionFactory;
        private readonly AnswerFactory _answerFactory;
        private readonly AnswerHandler _answerHandler;

        public QuestionInspector(QuestionFactory questionFactory, AnswerFactory answerFactory, AnswerHandler answerHandler)
        {
            _questionFactory = questionFactory;
            _answerFactory = answerFactory;
            _answerHandler = answerHandler;
        }

        /// <summary>
        /// Display question.
        /// </summary>
        /// <exception cref="UnassignedReferenceException">Exception tells you to add processing for this question type.</exception>
        public void DisplayQuestion(Question question)
        {
            QuestionView view = question switch
            {
                TextQuestion textQuestion => ShowTextView(textQuestion),
                _ => throw new UnassignedReferenceException("No such type registered in case")
            };

            _answerHandler.SetConfirmButton(view.ConfirmButton);
        }
        
        private void DisplayAnswer(Answer answer, Transform container)
        {
            switch (answer)
            {
                case TextAnswer textAnswer:
                    _answerFactory.CreateTextAnswer(textAnswer, container, _answerHandler);
                    break;
            }
        }

        private QuestionView ShowTextView(TextQuestion textQuestion)
        {
            var view = _questionFactory.ShowTextQuestion(textQuestion);
            ClearAndDisplayAnswers(textQuestion.Answers, view.Container);
            return view;
        }
        
        private void ClearAndDisplayAnswers(IEnumerable<Answer> answers, Transform container)
        {
            _answerFactory.ClearPreviousAnswers();
            
            foreach (Answer answer in answers) 
                DisplayAnswer(answer, container);
        }
    }
}