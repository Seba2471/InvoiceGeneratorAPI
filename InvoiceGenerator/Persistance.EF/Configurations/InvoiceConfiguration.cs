using InvoiceGenerator.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceGenerator.PersistanceEF.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.Property(i => i.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.IssueDate)
                .IsRequired();

            builder.HasOne(i => i.Seller)
                .WithMany(s => s.InvoicesSeller)
                .HasForeignKey(i => i.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(i => i.Buyer)
                .WithMany(b => b.InvoicesBuyer)
                .HasForeignKey(i => i.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
