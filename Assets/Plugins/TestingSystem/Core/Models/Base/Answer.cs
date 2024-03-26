namespace TestingSystem.Models
{
    /// <summary>
    /// Base answer model.
    /// </summary>
    public abstract class Answer
    {
        /// <summary>
        /// Answer unique id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the question this answer is related to.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Is Answer correct.
        /// </summary>
        public virtual bool IsCorrect { get; set; }
        
        public override bool Equals(object obj) => 
            obj is Answer answer && answer.Id == Id;

        public override int GetHashCode() => Id;
    }
}
