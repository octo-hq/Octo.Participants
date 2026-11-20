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

    public async Task<Participant?> GetByIdAsync(
        Guid id,
        QueryIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Participant> query = _context.Participants.AsNoTracking();

        query = ApplyIncludes(query, includeOptions);

        return await query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    private static IQueryable<TEntity> ApplyIncludes<TEntity>(
        IQueryable<TEntity> query,
        QueryIncludeOptions? includeOptions)
        where TEntity : class
    {
        if (includeOptions?.Includes is null || includeOptions.Includes.Count == 0)
            return query;

        foreach (var includePath in includeOptions.Includes)
        {
            if (!string.IsNullOrWhiteSpace(includePath))
            {
                query = query.Include(includePath.Trim());
            }
        }

        return query;
    }
}