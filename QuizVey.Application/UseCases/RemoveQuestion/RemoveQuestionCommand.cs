using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizVey.Application.UseCases.RemoveQuestion;

public record RemoveQuestionCommand(
    Guid AssessmentVersionId,
    Guid QuestionId
);