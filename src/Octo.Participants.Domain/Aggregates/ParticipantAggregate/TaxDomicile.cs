using Flunt.Notifications;
using Flunt.Validations;
using Octo.Core.Domain; // Onde deve estar a sua classe base ValueObject
using Octo.Core.Primitives;

namespace Octo.Participants.Domain.Aggregates.ParticipantAggregate;

public class TaxDomicile : ValueObject
{
    public string CountryCode { get; private set; }

    private TaxDomicile(string countryCode)
    {
        CountryCode = countryCode.ToUpper();
    }

    public static Result<TaxDomicile> Create(string countryCode)
    {
        var contract = new Contract<TaxDomicile>()
            .Requires()
            .IsNotNullOrEmpty(countryCode, "TaxDomicile.CountryCode", "Country code is required.")
            .AreEquals(countryCode?.Length ?? 0, 2, "TaxDomicile.CountryCode", "Country code must be a 2-letter ISO code.");

        if (!contract.IsValid)
        {
            // Retorna APENAS as notificações de erro. O objeto TaxDomicile NUNCA é instanciado.
            return Result<TaxDomicile>.Failure(contract);
        }

        // Retorna APENAS o objeto válido criado com sucesso.        
        return Result<TaxDomicile>.Success(new TaxDomicile(countryCode ?? ""));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CountryCode;
    }
}