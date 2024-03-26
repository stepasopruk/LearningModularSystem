#if SQLite
using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;

namespace TestingSystem.DTO
{
    public class QuestionResultDTO
    {
        [PrimaryKey, Indexed, AutoIncrement]
        public int Id { get; set; }

        public int TestResultId { get; set; }

        public int QuestionId { get; set; }

        public IEnumerable<int> GivenAnswersIds { get; set; }

        public string GivenAnswersJson
        {
            get => JsonConvert.SerializeObject(GivenAnswersIds); 
            set => GivenAnswersIds = JsonConvert.DeserializeObject<IEnumerable<int>>(value);
        }
    }
}
#endif