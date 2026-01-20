using Microsoft.EntityFrameworkCore;
using QuizVey.Api.Application.UseCases.CreateAssignment;
using QuizVey.Api.Application.UseCases.SubmitAttempt;
using QuizVey.Application.Interfaces;
using QuizVey.Application.UseCases.StartAttempt;
using QuizVey.Infrastructure.Persistence;
using QuizVey.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<IAssessmentVersionRepository, AssessmentVersionRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAttemptRepository, AttemptRepository>();

builder.Services.AddScoped<CreateAssignmentHandler>();
builder.Services.AddScoped<StartAttemptHandler>();
builder.Services.AddScoped<SubmitAttemptHandler>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
