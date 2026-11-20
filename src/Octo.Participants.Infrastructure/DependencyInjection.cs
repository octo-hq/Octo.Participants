using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;
using Octo.Participants.Infrastructure.Persistence;
using Octo.Participants.Infrastructure.Repositories;

namespace Octo.Participants.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {


        var connectionString = configuration.GetConnectionString("DefaultConnection");        
        var schema = configuration["Database:Schema"] ?? "participant";

        services.AddDbContext<ParticipantDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", schema)));

        // Registro do Repositório
        services.AddScoped<IParticipantRepository, ParticipantRepository>();

        return services;
    }
}