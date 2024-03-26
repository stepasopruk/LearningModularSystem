#if SQLite
using SQLite;

namespace TestingSystem.DTO
{
    public class AnswerDTO
    {
        [PrimaryKey, Indexed, AutoIncrement]
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
#endif