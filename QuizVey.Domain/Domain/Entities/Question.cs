using System;
using System.Collections.Generic;
using System.Linq;
using QuizVey.Domain.Enums;

namespace QuizVey.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; private set; }
        public string? Description { get; private set; }
        public QuestionType Type { get; private set; }
        public int Order { get; private set; }

        // ─────────────── BACKING FIELDS ───────────────

        private readonly List<string> _options = new();
        public IReadOnlyCollection<string> Options => _options.AsReadOnly();

        private readonly List<string> _correctAnswers = new();
        public IReadOnlyCollection<string> CorrectAnswers => _correctAnswers.AsReadOnly();

        // EF ONLY
        protected Question() { }

        public Question(string text, QuestionType type, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Question text cannot be empty.");

            Text = text;
            Type = type;
            Description = description;
        }

        // ─────────────── MODIFIERS ───────────────

        public void SetOrder(int order)
        {
            Order = order;
        }

        public void SetOptions(IEnumerable<string> options, IEnumerable<string> correct)
        {
            if (Type == QuestionType.FreeText)
                throw new InvalidOperationException("Free text questions cannot have options.");

            if (options == null || !options.Any())
                throw new InvalidOperationException("Options cannot be empty.");

            if (correct == null || !correct.Any())
                throw new InvalidOperationException("Correct answers required.");

            if (!correct.All(options.Contains))
                throw new InvalidOperationException("Correct answers must be part of options.");

            _options.Clear();
            _options.AddRange(options);

            _correctAnswers.Clear();
            _correctAnswers.AddRange(correct);
        }

        // Clone for version duplication
        public Question Clone()
        {
            var clone = new Question(Text, Type, Description);

            clone._options.AddRange(_options);
            clone._correctAnswers.AddRange(_correctAnswers);

            return clone;
        }

        // ─────────────── EVALUATION ───────────────

        public bool IsCorrect(string submittedValue)
        {
            if (Type == QuestionType.FreeText)
                throw new InvalidOperationException("Free-text questions are not evaluated.");

            if (string.IsNullOrWhiteSpace(submittedValue))
                return false;

            return Type switch
            {
                QuestionType.SingleChoice 
                or QuestionType.TrueFalse 
                or QuestionType.YesNo
                    => _correctAnswers.Contains(submittedValue),

                QuestionType.MultipleChoice
                    => _correctAnswers.OrderBy(x => x)
                        .SequenceEqual(
                            submittedValue.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(x => x.Trim())
                                          .OrderBy(x => x)
                        ),

                _ => throw new InvalidOperationException($"Unsupported type {Type}")
            };
        }
    }
}
