using TestingSystem.Models;

namespace TestingSystem.Example
{
    public class ExampleTest
    {
        public readonly Test Test = new Test()
        {
            Id = 1,
            Name = "Test 1",
            Questions = new Question[]
            {
                new TextQuestion()
                {
                    Answers = new Answer[]
                    {
                        new TextAnswer()
                        {
                            Id = 0,
                            IsCorrect = true,
                            Text = "Correct"
                        },
                        new TextAnswer()
                        {
                            Id = 1,
                            IsCorrect = false,
                            Text = "Wrong"
                        },
                        new TextAnswer()
                        {
                            Id = 2,
                            IsCorrect = false,
                            Text = "Wrong"
                        },
                        new TextAnswer()
                        {
                            Id = 3,
                            IsCorrect = false,
                            Text = "Wrong"
                        },

                    },
                    Id = 0,
                    Text = "Question 1",
                    Weight = 1,
                },
                new TextQuestion()
                {
                    Answers = new Answer[]
                    {
                        new TextAnswer()
                        {
                            Id = 4,
                            IsCorrect = true,
                            Text = "Correct"
                        },
                        new TextAnswer()
                        {
                            Id = 5,
                            IsCorrect = true,
                            Text = "Correct"
                        },
                        new TextAnswer()
                        {
                            Id = 6,
                            IsCorrect = true,
                            Text = "Correct"
                        },
                        new TextAnswer()
                        {
                            Id = 7,
                            IsCorrect = true,
                            Text = "Correct"
                        },

                    },
                    Id = 1,
                    Text = "Question 2",
                    Weight = 1,
                },
            }
        };
    }
}