using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Domain.Enums;

namespace QuizVey.Application.UseCases.CreateQuestion;

public record CreateQuestionCommand(
    Guid AssessmentVersionId,
    string Text,
    QuestionType Type,
    string? Description,
    IEnumerable<string>? Options,          // ALL available choices
    IEnumerable<string>? CorrectAnswers    // Subset of options
);