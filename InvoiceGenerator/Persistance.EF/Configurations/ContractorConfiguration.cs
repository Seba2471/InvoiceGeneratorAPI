using InvoiceGenerator.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.PersistanceEF.Configurations
{
    public class ContractorConfiguration : IEntityTypeConfiguration<Contractor>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Contractor> builder)
        {
            builder.HasOne(c => c.Address)
                .WithOne(u => u.Contractor);

            //builder.HasMany(c => c.Invoices)
            //    .WithOne(i => i.Seller);

            //builder.HasMany(c => c.Invoices)
            //    .WithOne(i => i.Buyer);
        }
    }
}
