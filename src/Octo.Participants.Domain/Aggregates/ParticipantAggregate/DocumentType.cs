namespace Octo.Participants.Domain.Aggregates.ParticipantAggregate;

public enum DocumentType
{
    IndividualTaxId = 1,  // CPF (BR), SSN (US), etc.
    CorporateTaxId = 2,   // CNPJ (BR), EIN (US), VAT (EU), etc.
    Passport = 3,
    ForeignIdentity = 4   // RNE, National ID, etc.
}