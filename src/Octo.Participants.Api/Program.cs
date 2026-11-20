using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Octo.Participants.Api.Endpoints;
using Octo.Participants.Application;
using Octo.Participants.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Octo.Participants API",
            Version = "v1",
            Description = "Microsserviço de gestão de participantes e vínculos do ecossistema Octo."
        };

        return Task.CompletedTask;
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Expõe /openapi/v1.json
    app.MapOpenApi();

    // Expõe a UI interativa do Scalar (padrão em /scalar/v1)
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Octo.Participants — API Reference")
            .WithTheme(ScalarTheme.Purple) // ex.: Default, Alternate, Moon, Purple, Solarized, etc.
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapParticipantEndpoints();
app.MapControllers();

app.Run();
