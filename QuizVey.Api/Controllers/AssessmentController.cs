using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using QuizVey.Api.Contracts.Assessment;
using QuizVey.Application.UseCases.CreateAssessment;

namespace QuizVey.Api.Controllers;

[ApiController]
[Route("api/assessment")]
public class AssessmentsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssessmentRequest request,
        [FromServices] CreateAssessmentHandler handler)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Assessment title cannot be empty");
        }

        var command = new CreateAssessmentCommand(
            request.Title,
            request.Description,
            request.Type
        );

        var result = await handler.Handle(command);

        return CreatedAtAction(
            nameof(GetById),
            new {id = result.Id},
            result
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok();
    }
}
