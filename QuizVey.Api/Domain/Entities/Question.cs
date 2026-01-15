using System;
using System.Collections.Generic;
using System.Linq;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; private set; }
        public string? Description { get; private set; }
        public QuestionType Type { get; private set; }

        private readonly List<string> _correctAnswers = new();
        public IReadOnlyCollection<string> CorrectAnswers => _correctAnswers.AsReadOnly();

        protected Question() { }

        public Question(string text, QuestionType type, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Question text cannot be empty.");

            Text = text;
            Type = type;
            Description = description;
        }

        public Question Clone()
        {
            var clone = new Question(Text, Type, Description);

            foreach (var answer in _correctAnswers)
                clone._correctAnswers.Add(answer);

            return clone;
        }

        public void SetCorrectAnswers(IEnumerable<string> answers)
        {
            if (Type == QuestionType.FreeText)
                throw new InvalidOperationException(
                    "Free-text questions cannot have correct answers."
                );

            if (answers == null || !answers.Any())
                throw new InvalidOperationException(
                    "At least one correct answer must be provided."
                );

            _correctAnswers.Clear();
            _correctAnswers.AddRange(answers);
        }

        public bool IsCorrect(string submittedValue)
        {
            if (Type == QuestionType.FreeText)
                throw new InvalidOperationException(
                    "Free-text questions are not evaluated."
                );

            if (string.IsNullOrWhiteSpace(submittedValue))
                return false;

            return Type switch
            {
                QuestionType.SingleChoice
                or QuestionType.TrueFalse
                or QuestionType.YesNo
                    => _correctAnswers.Contains(submittedValue),

                QuestionType.MultipleChoice
                    => _correctAnswers
                        .OrderBy(x => x)
                        .SequenceEqual(
                            submittedValue
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .OrderBy(x => x)
                        ),

                _ => throw new InvalidOperationException(
                    $"Unsupported question type: {Type}"
                )
            };
        }
    }
}
