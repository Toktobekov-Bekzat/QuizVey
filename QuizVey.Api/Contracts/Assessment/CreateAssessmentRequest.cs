using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Domain.Enums;

namespace QuizVey.Api.Contracts.Assessment;
public record CreateAssessmentRequest(
    string Title,
    string? Description,
    AssessmentType Type
);