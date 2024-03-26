using TestingSystem.Models;
using UnityEngine;

namespace TestingSystem.DTSo
{
    /// <summary>
    /// Base implementation for answer SO (data transfer object).
    /// </summary>
    public abstract class AnswerSCO : ScriptableObject
    {
        [SerializeField] protected int id;

        /// <summary>
        /// Get answer.
        /// </summary>
        /// <returns>Answer.</returns>
        public abstract Answer GetAnswer();
    }
}