using TestingSystem.Models;
using UnityEngine;

namespace TestingSystem.DTSo
{
    /// <summary>
    /// Base implementation for question SO (data transfer object).
    /// </summary>
    public abstract class QuestionSCO : ScriptableObject
    {
        [SerializeField] protected int id;
        [SerializeField] protected int weight;
        [SerializeField] protected AnswerSCO[] answers;
        
        /// <summary>
        /// Get Question.
        /// </summary>
        /// <returns>Question.</returns>
        public abstract Question GetQuestion();
    }
}