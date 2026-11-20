using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octo.Participants.Application.Participants.Commands.RegisterParticipant;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Octo.Participants.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RegisterParticipantCommandHandler>();
        return services;
    }
}
