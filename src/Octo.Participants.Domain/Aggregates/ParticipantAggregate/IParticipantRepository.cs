namespace Octo.Participants.Domain.Aggregates.ParticipantAggregate;

public interface IParticipantRepository
{
    Task AddAsync(Participant participant, CancellationToken cancellationToken);
    Task<Participant?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // Outros métodos que façam sentido para o agregado futuramente
    // Task<bool> ExistsByDocumentAsync(string documentNumber, CancellationToken cancellationToken);
}