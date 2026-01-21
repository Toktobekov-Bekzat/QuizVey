using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizVey.Application.UseCases.CreateAssessment;

public record CreateAssessmentCommand(
    string Title,
    string? Description
);