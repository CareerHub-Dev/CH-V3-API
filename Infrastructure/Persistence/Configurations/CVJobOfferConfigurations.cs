using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CVJobOfferConfigurations : IEntityTypeConfiguration<CVJobOffer>
{
    public void Configure(EntityTypeBuilder<CVJobOffer> builder)
    {

    }
}
