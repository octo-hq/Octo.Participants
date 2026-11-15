using Octo.Participants.Domain.Aggregates.ParticipantAggregate;

public record RegisterParticipantCommand(
    string Name,
    string PrimaryEmail,
    string DocumentNumber,
    DocumentType DocumentType, // "Individual" ou "Corporate"
    string CountryCode   // "BR", "US", etc.
);