namespace Octo.Participants.Domain.Aggregates.ParticipantAggregate;

public sealed record QueryIncludeOptions(IReadOnlyCollection<string>? Includes = null)
{
    public static readonly QueryIncludeOptions Empty = new();
}

public interface IParticipantRepository
{
    Task AddAsync(Participant participant, CancellationToken cancellationToken);
    Task<Participant?> GetByIdAsync(
        Guid id,
        QueryIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default);

    // Outros métodos que façam sentido para o agregado futuramente
    // Task<bool> ExistsByDocumentAsync(string documentNumber, CancellationToken cancellationToken);
}