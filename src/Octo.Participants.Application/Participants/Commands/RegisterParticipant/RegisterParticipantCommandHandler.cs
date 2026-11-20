using Flunt.Validations;
using Octo.Core.Primitives;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;
using System.Xml.Linq;

namespace Octo.Participants.Application.Participants.Commands.RegisterParticipant;

public class RegisterParticipantCommandHandler
{
    private readonly IParticipantRepository _participantRepository;
    // private readonly IUnitOfWork _unitOfWork; // Se aplicável para commit

    public RegisterParticipantCommandHandler(IParticipantRepository participantRepository)
    {
        _participantRepository = participantRepository;
    }

    public async Task<Result<Guid>> HandleAsync(RegisterParticipantCommand command, CancellationToken cancellationToken)
    {                
        // O domínio faz todo o trabalho pesado de validação e composição em um único lugar
        var participantResult = Participant.Create(
            command.Name,
            command.PrimaryEmail,
            command.DocumentNumber,
            command.DocumentType,
            command.CountryCode
        );

        // Se o domínio retornou falha, repassamos as notificações direto para a API/Front-end
        if (!participantResult.IsValid)
        {
            return Result<Guid>.Failure(participantResult);
        }

        // Sucesso absoluto: persiste o agregado válido
        var participant = participantResult.Value;
        await _participantRepository.AddAsync(participant!, cancellationToken);

        return Result<Guid>.Success(participant!.Id);
    }
}