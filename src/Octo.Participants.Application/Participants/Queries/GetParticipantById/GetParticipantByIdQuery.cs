namespace Octo.Participants.Application.Participants.Queries.GetParticipantById;

public sealed record GetParticipantByIdQuery(Guid Id);

public sealed record ParticipantDetailsResponse(
    Guid Id,
    string Name,
    string PrimaryEmail,
    ParticipantDocumentResponse PrimaryDocument
);

public sealed record ParticipantDocumentResponse(
    string Number,
    string Type,
    string CountryCode
);