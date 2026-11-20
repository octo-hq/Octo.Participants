using Flunt.Validations;
using Octo.Core.Domain;
using Octo.Core.Primitives;

namespace Octo.Participants.Domain.Aggregates.ParticipantAggregate;

public class Participant : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public IdentityDocument PrimaryDocument { get; private set; }
    public string PrimaryEmail { get; private set; }

    private Participant() { }
    private Participant(string name, IdentityDocument primaryDocument, string primaryEmail)
        : base(Guid.NewGuid())
    {
        Name = name;
        PrimaryDocument = primaryDocument;
        PrimaryEmail = primaryEmail;
    }

    public static Result<Participant> Create(
        string name,
        string primaryEmail,
        string documentNumber,
        DocumentType documentType,
        string countryCode)
    {
        // 1. Tenta criar o Domicílio Fiscal
        var taxDomicileResult = TaxDomicile.Create(countryCode);
        if (!taxDomicileResult.IsValid)
        {
            // Se falhou, retorna o erro do domicílio imediatamente
            return Result<Participant>.Failure(taxDomicileResult);
        }

        // 2. Tenta criar o Documento de Identidade usando o VO gerado com sucesso
        var identityDocResult = IdentityDocument.Create(documentNumber, documentType, taxDomicileResult.Value);
        if (!identityDocResult.IsValid)
        {
            return Result<Participant>.Failure(identityDocResult);
        }

        // 3. Valida os dados do próprio Participante
        var contract = new Contract<Participant>()
            .Requires()
            .IsNotNullOrEmpty(name, "Participant.Name", "Name is required.")
            .IsEmail(primaryEmail, "Participant.PrimaryEmail", "Invalid primary email address.");

        if (!contract.IsValid)
        {
            return Result<Participant>.Failure(contract);
        }

        // 4. Se chegou até aqui, TUDO é válido. Instancia o agregado com 100% de garantia.
        var participant = new Participant(name, identityDocResult.Value, primaryEmail);
        return Result<Participant>.Success(participant);
    }
}