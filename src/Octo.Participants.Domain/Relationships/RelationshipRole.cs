using Flunt.Validations;
using Octo.Core.Domain;
using Octo.Core.Primitives;

namespace Octo.Participants.Domain.Relationships;

public class RelationshipRole : Entity<Guid>
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; } // Ex: "SHAREHOLDER", "DIRECTOR"
    public bool IsActive { get; private set; }

    private RelationshipRole(Guid tenantId, string name, string code) : base(Guid.NewGuid())
    {
        TenantId = tenantId;
        Name = name;
        Code = code.ToUpperInvariant();
        IsActive = true;
    }

    public static Result Create(Guid tenantId, string name, string code)
    {
        var contract = new Contract<RelationshipRole>()
            .Requires()
            .AreNotEquals(tenantId, Guid.Empty, "RelationshipRole.TenantId", "Tenant ID cannot be empty.")
            .IsNotNullOrEmpty(name, "RelationshipRole.Name", "Role name is required.")
            .IsNotNullOrEmpty(code, "RelationshipRole.Code", "Role code is required.");

        if (!contract.IsValid)
            return Result.Failure(contract);

        var role = new RelationshipRole(tenantId, name, code);
        return Result<RelationshipRole>.Success(role);
    }

    public void UpdateDetails(string name)
    {
        Name = name;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}