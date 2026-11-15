using Flunt.Validations;
using Octo.Core.Domain;
using Octo.Core.Primitives;

namespace Octo.Participants.Domain.Aggregates.ParticipantAggregate;

public class IdentityDocument : ValueObject
{
    public string Number { get; private set; }
    public DocumentType Type { get; private set; }
    public TaxDomicile Domicile { get; private set; }

    private IdentityDocument(string number, DocumentType type, TaxDomicile domicile)
    {
        Number = number;
        Type = type;
        Domicile = domicile;
    }

    public static Result<IdentityDocument> Create(string number, DocumentType type, TaxDomicile domicile)
    {
        var contract = new Contract<IdentityDocument>()
            .Requires()
            .IsNotNullOrEmpty(number, "IdentityDocument.Number", "Document number cannot be empty.");

        if (domicile is null)
        {
            return Result<IdentityDocument>.Failure(contract);
        }

        if (domicile.CountryCode == "BR")
        {
            if (type == DocumentType.IndividualTaxId)
                contract.IsTrue(ValidateCpf(number), "IdentityDocument.Number", "Invalid CPF for Brazilian tax domicile.");
            else if (type == DocumentType.CorporateTaxId)
                contract.IsTrue(ValidateCnpj(number), "IdentityDocument.Number", "Invalid CNPJ for Brazilian tax domicile.");
        }

        if (!contract.IsValid)
            return Result<IdentityDocument>.Failure(contract);

        return Result<IdentityDocument>.Success(new IdentityDocument(number, type, domicile));
    }

    private static bool ValidateCpf(string cpf) => true;
    private static bool ValidateCnpj(string cnpj) => true;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return Type;
        yield return Domicile;
    }
}