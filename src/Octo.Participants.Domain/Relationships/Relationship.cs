using Flunt.Validations;
using Octo.Core.Domain;
using Octo.Core.Primitives;

namespace Octo.Participants.Domain.Relationships;

public class Relationship : Entity<Guid>
{
    public Guid ParticipantId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid RoleId { get; private set; } // Aponta para a entidade RelationshipRole do Tenant
    public bool IsActive { get; private set; }
    public DateTime EstablishedAt { get; private set; }

    private Relationship(Guid participantId, Guid tenantId, string role) : base(Guid.NewGuid())
    {
        ParticipantId = participantId;
        TenantId = tenantId;
        RoleId = Guid.Parse(role);
        IsActive = true;
        EstablishedAt = DateTime.UtcNow;
    }

    public static Result Create(Guid participantId, Guid tenantId, string role)
    {
        var contract = new Contract<Relationship>()
            .Requires()
            .AreNotEquals(participantId, Guid.Empty, "Relationship.ParticipantId", "Participant ID cannot be empty.")
            .AreNotEquals(tenantId, Guid.Empty, "Relationship.TenantId", "Tenant ID cannot be empty.")
            .IsNotNullOrEmpty(role, "Relationship.Role", "Relationship role is required.");

        if (!contract.IsValid)
        {
            return Result.Failure(contract);
        }

        var relationship = new Relationship(participantId, tenantId, role);
        return Result<Relationship>.Success(relationship);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}