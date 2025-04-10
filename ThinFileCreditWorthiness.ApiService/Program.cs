using Microsoft.OpenApi.Models;
using Microsoft.SemanticKernel;
using ThinFileCreditWorthiness.ApiService.Agents;
using ThinFileCreditWorthiness.ApiService.Services;

#pragma warning disable SKEXP0001

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped(sp =>
{
    var deploymentName = builder.Configuration["Parameters:deploymentName"];
    var endpoint = builder.Configuration["Parameters:endpoint"];
    var apiKey = builder.Configuration["Parameters:apiKey"];

    var kernel = Kernel.CreateBuilder()
        .AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey)
        .Build();

    kernel.Plugins.AddFromType<CreditDecisionPlugin>("CreditDecisionPlugin");

    return kernel;
});

builder.Services.AddScoped<BorrowerDataCollectionAgent>();
builder.Services.AddScoped<FraudDetectionAgent>();
builder.Services.AddScoped<PQIInspectorAgent>();
builder.Services.AddScoped<BCIInspectorAgent>();
builder.Services.AddScoped<CreditDecisionAgent>();

builder.Services.AddScoped<AgentExecutionManager>();
builder.Services.AddScoped<CreditEvaluationService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ThinFileCreditWorthiness API", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapSwagger();
app.MapDefaultEndpoints();

app.Run();