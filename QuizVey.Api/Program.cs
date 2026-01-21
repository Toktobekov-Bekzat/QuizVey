using Microsoft.EntityFrameworkCore;
using QuizVey.Application.UseCases.CreateAssignment;
using QuizVey.Application.UseCases.SubmitAttempt;
using QuizVey.Application.Interfaces;
using QuizVey.Application.UseCases.StartAttempt;
using QuizVey.Infrastructure.Persistence;
using QuizVey.Infrastructure.Repositories;
using QuizVey.Application.UseCases.CreateAssessment;
using QuizVey.Application.UseCases.CreateAssessmentVersion;

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
builder.Services.AddScoped<CreateAssessmentHandler>();
builder.Services.AddScoped<CreateAssessmentVersionHandler>();

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
