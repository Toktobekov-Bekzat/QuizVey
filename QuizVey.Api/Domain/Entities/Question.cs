using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizVey.Api.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; private set; }

        public string? Description { get; private set; }

        public QuestionType Type { get; private set; }

        protected Question(){}

        public Question(string text, string? description = null, QuestionType type)
        {
            Text = text;
            Description = description;
            Type = type;
        }

        public Question Clone()
        {
            return new Questions(Text, Type, Description);
        }
    }
}