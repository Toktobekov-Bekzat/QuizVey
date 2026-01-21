using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Domain.Enums;

namespace QuizVey.Application.UseCases.UpdateQuestion;

public record UpdateQuestionResult(
    Guid QuestionId
    );
