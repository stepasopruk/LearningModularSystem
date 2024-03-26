using System.Collections.Generic;
using System.Linq;
using TestingSystem.Models;
using UnityEngine;

namespace TestingSystem.DTSo
{
    /// <summary>
    /// Implementation for test SO (data transfer object).
    /// </summary>
    public sealed class TestSCO : ScriptableObject
    {
        [SerializeField] private string testName;
        [SerializeField] private int id;
        [SerializeField] private QuestionSCO[] questions;

        private Test _test;

        /// <summary>
        /// Get test.
        /// </summary>
        /// <returns>Test.</returns>
        public Test GetTest()
        {
            if (_test != null)
                return _test;
            
            IEnumerable<Question> questionCollection = questions.Select(x => x.GetQuestion());
            
            return _test = new Test()
            {
                Id = id,
                Name = testName,
                Questions = questionCollection
            };
        }
    }
}