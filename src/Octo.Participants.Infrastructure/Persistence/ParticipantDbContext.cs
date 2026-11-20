using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;

namespace Octo.Participants.Infrastructure.Persistence;

public class ParticipantDbContext : DbContext
{
    private readonly string _schema;

    public DbSet<Participant> Participants => Set<Participant>();

    public ParticipantDbContext(
        DbContextOptions<ParticipantDbContext> options,
        IConfiguration? configuration = null) : base(options)
    {
        _schema = configuration?["Database:Schema"] ?? "participant";
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(_schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParticipantDbContext).Assembly);

        // Ignora globalmente a propriedade 'Notifications' apenas em entidades raiz (não owned types)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetProperty("Notifications") != null)
            {
                entityType.AddIgnored("Notifications");
            }
        }
    }
}