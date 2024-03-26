using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestingSystem.Models
{
    /// <summary>
    /// Result for question
    /// </summary>
    public sealed class QuestionResult
    {
        /// <summary>
        /// Question result unique id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Question related to result.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// Given answers for this question.
        /// </summary>
        public IEnumerable<Answer> GivenAnswers { get; set; }

        /// <summary>
        /// Calculate result.
        /// </summary>
        /// <returns>Weight of question if correct or 0 if wrong.</returns>
        public int CheckAnswers()
        {
            return Question.Answers.Where(a => a.IsCorrect).
                Any(correctAnswer => !GivenAnswers.Contains(correctAnswer)) ? 0 : Question.Weight;
        }    
    }
}
