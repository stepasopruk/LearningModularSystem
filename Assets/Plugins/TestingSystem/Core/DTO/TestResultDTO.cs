#if SQLite
using SQLite;

namespace TestingSystem.DTO
{
    public class TestResultDTO
    {
        [PrimaryKey, Indexed, AutoIncrement]
        public int Id { get; set; }

        public int TestId { get; set; }

        public int StudentId { get; set; }        
    }
}
#endif