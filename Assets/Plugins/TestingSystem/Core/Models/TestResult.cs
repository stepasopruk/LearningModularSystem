using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestingSystem.Models
{
    /// <summary>
    /// Results for test
    /// </summary>
    public sealed class TestResult
    {
        /// <summary>
        /// Self identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the test to which the result is associated
        /// </summary>
        public int TestId { get; set; }

        /// <summary>
        /// Question results for test questions;
        /// </summary>
        public IEnumerable<QuestionResult> QuestionResults { get; set; }

        /// <summary>
        /// Calculate points (weight) earned in test (question results)
        /// </summary>
        /// <returns>Sum of questions weight</returns>
        public int GetRating() => QuestionResults.Sum(questionResult => questionResult.CheckAnswers());
    }
}
