using System.Collections.Generic;

namespace TestingSystem.Models
{
    /// <summary>
    /// Base model for Question.
    /// </summary>
    public abstract class Question
    {
        /// <summary>
        /// Question unique id.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Answers for this question.
        /// </summary>
        public IEnumerable<Answer> Answers { get; set; }

        /// <summary>
        /// Question weight (or the points count that will be received with a correct answer).
        /// </summary>
        public int Weight { get; set; }
    }
}
