using Microsoft.EntityFrameworkCore;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;

namespace Octo.Participants.Infrastructure.Persistence;

public class ParticipantDbContext : DbContext
{
    public DbSet<Participant> Participants => Set<Participant>();

    public ParticipantDbContext(DbContextOptions<ParticipantDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas as configurações de mapeamento (ParticipantConfiguration, etc.) do assembly atual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParticipantDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}