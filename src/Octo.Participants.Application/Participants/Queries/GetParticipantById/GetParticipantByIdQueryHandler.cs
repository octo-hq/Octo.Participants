using Flunt.Notifications;
using Flunt.Validations;
using Octo.Core.Primitives;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;

namespace Octo.Participants.Application.Participants.Queries.GetParticipantById;

public sealed class GetParticipantByIdQueryHandler
{
    private readonly IParticipantRepository _participantRepository;

    public GetParticipantByIdQueryHandler(IParticipantRepository participantRepository)
    {
        _participantRepository = participantRepository;
    }

    public async Task<Result<ParticipantDetailsResponse>> HandleAsync(
        GetParticipantByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var includeOptions = new QueryIncludeOptions(
        [
            "Relationships",
            "Relationships.Details"
        ]);

        var participant = await _participantRepository.GetByIdAsync(
            query.Id,
            includeOptions: null,
            cancellationToken);

        if (participant is null)
        {
            var contract = new Contract<Participant>();                
            contract.AddNotification("Participant.Id", "Participant not found.");

            return Result<ParticipantDetailsResponse>.Failure(contract);
        }

        var response = new ParticipantDetailsResponse(
            participant.Id,
            participant.Name,
            participant.PrimaryEmail,
            new ParticipantDocumentResponse(
                participant.PrimaryDocument.Number,
                participant.PrimaryDocument.Type.ToString(),
                participant.PrimaryDocument.Domicile.CountryCode
            )
        );

        return Result<ParticipantDetailsResponse>.Success(response);
    }
}