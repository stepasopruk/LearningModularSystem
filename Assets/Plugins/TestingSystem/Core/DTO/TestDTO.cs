#if SQLite
using SQLite;

namespace TestingSystem.DTO
{
    public class TestDTO
    {
        [PrimaryKey, Indexed, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }        
    }
}
#endif