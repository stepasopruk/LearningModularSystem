#if SQLite
using SQLite;

namespace TestingSystem.DTO
{
    public class QuestionDTO
    {
        [PrimaryKey, Indexed, AutoIncrement]
        public int Id { get; set; }

        public int TestId { get; set; }

        public float Weight { get; set; }
    }
}
#endif