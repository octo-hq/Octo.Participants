using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octo.Participants.Domain.Aggregates.ParticipantAggregate;

namespace Octo.Participants.Infrastructure.Persistence.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participants");

        builder.HasKey(p => p.Id);

        builder.Ignore(p => p.Notifications);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.PrimaryEmail)
            .IsRequired()
            .HasMaxLength(250);

        // Mapeia o PrimaryDocument (IdentityDocument) que pertence ao Participant
        builder.OwnsOne(p => p.PrimaryDocument, doc =>
        {
            doc.WithOwner(); // Força explicitamente a relação de propriedade

            doc.Property(d => d.Number)
                .HasColumnName("DocumentNumber")
                .IsRequired()
                .HasMaxLength(50);

            doc.Property(d => d.Type)
                .HasColumnName("DocumentType")
                .IsRequired();

            // Mapeia o TaxDomicile que está dentro do IdentityDocument
            doc.OwnsOne(d => d.Domicile, tax =>
            {
                tax.WithOwner(); // Força explicitamente a propriedade aninhada

                tax.Property(t => t.CountryCode)
                    .HasColumnName("DocumentTaxCountryCode")
                    .IsRequired()
                    .HasMaxLength(2);
            });
        });
    }
}