using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizVey.Application.UseCases.UpdateQuestion;
public record UpdateQuestionCommand
(
    Guid AssessmentVersionId,
    Guid QuestionId,
    string Text,
    string? Description,
    QuestionType Type,
    IEnumerable<string> Options,
    IEnumerable<string> CorrectAnswers,
    int Order
);
