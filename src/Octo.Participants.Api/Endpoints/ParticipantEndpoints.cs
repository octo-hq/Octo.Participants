using Microsoft.EntityFrameworkCore;
using Octo.Participants.Application.Participants.Commands.RegisterParticipant;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;
using Octo.Participants.Infrastructure.Persistence;

namespace Octo.Participants.Api.Endpoints;

public static class ParticipantEndpoints
{    
    public static IEndpointRouteBuilder MapParticipantEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/participants")
            .WithTags("Participants");

        group.MapPost("/", async (
            RegisterParticipantRequest request,
            RegisterParticipantCommandHandler handler,
            CancellationToken cancellationToken) =>
        {

            if (!Enum.TryParse<DocumentType>(request.DocumentType, ignoreCase: true, out var documentType))
            {
                return Results.BadRequest(new
                {
                    errors = new[] { new { key = "DocumentType", message = "Invalid document type." } }
                });
            }

            var command = new RegisterParticipantCommand(
                request.Name,
                request.PrimaryEmail,
                request.DocumentNumber,
                documentType,
                request.CountryCode
            );

            var result = await handler.HandleAsync(command, cancellationToken);

            if (!result.IsValid)
            {
                return Results.BadRequest(new
                {
                    errors = result.Notifications
                });
            }

            return Results.Created($"/api/v1/participants/{result.Value}", new
            {
                id = result.Value
            });
        })
        .WithName("RegisterParticipant")
        .WithSummary("Cadastra um novo participante")
        .WithDescription("Valida os dados do participante e seus value objects de documento e domicílio fiscal.")
        .Produces<RegisterParticipantResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", async (
            Guid id,
            ParticipantDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var participant = await dbContext.Participants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (participant is null)
                return Results.NotFound();

            return Results.Ok(participant);
        })
        .WithName("GetParticipantById")
        .WithSummary("Busca um participante por ID")
        .WithDescription("Retorna os dados completos do participante")
        .Produces<Participant>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}

public sealed record RegisterParticipantRequest(
    string Name,
    string PrimaryEmail,
    string DocumentNumber,
    string DocumentType,
    string CountryCode
);