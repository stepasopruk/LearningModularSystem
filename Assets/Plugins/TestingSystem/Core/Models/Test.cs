using System.Collections.Generic;

namespace TestingSystem.Models
{
    /// <summary>
    /// Test model.
    /// </summary>
    public sealed class Test
    {
        /// <summary>
        /// Test unique id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Test name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Questions stored in this test 
        /// </summary>
        public IEnumerable<Question> Questions { get; set; }
    }
}
