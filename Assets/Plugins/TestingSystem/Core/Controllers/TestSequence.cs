using System;
using System.Collections.Generic;
using TestingSystem.Models;

namespace TestingSystem.Controllers
{
    /// <summary>
    /// Responsible for handling questions and answers
    /// </summary>
    public class TestSequence
    {
        /// <summary>
        /// Callback from test finish
        /// </summary>
        public event Action<TestResult> OnTestResultReceived;
        
        private readonly QuestionInspector _questionInspector;
        private readonly Dictionary<Question, QuestionResult> _questionResults;
        
        private LinkedListNode<Question> _currentQuestion;
        private Test _currentTest;

        public TestSequence(QuestionInspector questionInspector)
        {
            _questionInspector = questionInspector;
            _questionResults = new Dictionary<Question, QuestionResult>();
        }

        /// <summary>
        /// Starts a test.
        /// </summary>
        /// <param name="test">Test to be started.</param>
        public void Start(Test test)
        {
            _currentTest = test;
            _questionResults.Clear();
            
            var questions = new LinkedList<Question>(test.Questions);
            _currentQuestion = questions.First;
            
            ShowQuestion(_currentQuestion);
        }
        
        /// <summary>
        /// Saves answers for current questions.
        /// </summary>
        public void PushAnswer(IEnumerable<Answer> answers)
        {
            WriteAnswer(answers);
            ShowNextOrFinish();
        }

        private void ShowNextOrFinish()
        {
            if (_currentQuestion.Next != null)
                ShowQuestion(_currentQuestion.Next);
            else
                FinishTest();
        }

        private void ShowQuestion(LinkedListNode<Question> question)
        {
            _questionInspector.DisplayQuestion(question.Value);
            _currentQuestion = question;
        }

        private void FinishTest()
        {
            var result = new TestResult
            {
                TestId = _currentTest.Id,
                QuestionResults = _questionResults.Values
            };

            OnTestResultReceived?.Invoke(result);
        }
        
        private void WriteAnswer(IEnumerable<Answer> answers)
        {
            
            Question question = _currentQuestion.Value;
            QuestionResult questionResult = CalculateResult(question, answers);

            if (_questionResults.ContainsKey(question))
                _questionResults[question] = questionResult;
            else
                _questionResults.Add(question, questionResult);
        }
        
        private QuestionResult CalculateResult(Question question, IEnumerable<Answer> answers)
        {
            return new QuestionResult()
            {
                Question = question,
                GivenAnswers = answers
            };
        }
    }
}