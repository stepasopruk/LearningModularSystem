using System;
using System.Collections.Generic;
using TestingSystem.Factories;
using TestingSystem.Models;
using TestingSystem.Views;
using TestingSystem.Views.Data;
using UnityEngine;

namespace TestingSystem.Controllers
{
    /// <summary>
    /// The test controller, which is responsible for initializing and running the test, monitors its completion.
    /// </summary>
    public class TestController : MonoBehaviour
    {
        /// <summary>
        /// Finish test callback.
        /// </summary>
        public event Action<TestResult> OnTestFinished; 

        [SerializeField] private QuestionView[] questionViews;
        [SerializeField] private AnswersPrefabs answersPrefabs;
        
        private TestSequence _testSequence;
        private AnswerHandler _answerHandler;
        private QuestionInspector _questionInspector;

        private void Awake() => Initialize();

        private void OnDestroy()
        {
            _testSequence.OnTestResultReceived -= FinishTest;
            _answerHandler?.Dispose();
        }

        /// <summary>
        /// Starts test
        /// </summary>
        /// <param name="test">Test to be started</param>
        public void StartTest(Test test) => _testSequence.Start(test);

        /// <summary>
        /// Get answers and submit them for validation
        /// </summary>
        /// <param name="answers"></param>
        public void ReceiveAnswer(IEnumerable<Answer> answers) => _testSequence.PushAnswer(answers);

        private void FinishTest(TestResult result) => OnTestFinished?.Invoke(result);
        
        private void Initialize()
        {
            var answerDemonstrator = new AnswerFactory(answersPrefabs);
            var questionDemonstrator = new QuestionFactory(questionViews);
            
            _answerHandler = new AnswerHandler(this);
            _questionInspector = new QuestionInspector(questionDemonstrator, answerDemonstrator, _answerHandler);
            _testSequence = new TestSequence(_questionInspector);
            
            _testSequence.OnTestResultReceived += FinishTest;
        }
    }
}