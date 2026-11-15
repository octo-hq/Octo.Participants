using Microsoft.EntityFrameworkCore;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;
using Octo.Participants.Infrastructure.Persistence;

namespace Octo.Participants.Infrastructure.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly ParticipantDbContext _context;

    public ParticipantRepository(ParticipantDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Participant participant, CancellationToken cancellationToken)
    {
        await _context.Participants.AddAsync(participant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Participant?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Participants
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}