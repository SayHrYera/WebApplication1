using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Configurations
{
    public class BillDetailsConfig : IEntityTypeConfiguration<BillDetails>
    {
        public void Configure(EntityTypeBuilder<BillDetails> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Bill).WithMany(p => p.Details).HasForeignKey(p => p.BillId);
            builder.HasOne(p => p.Product).WithMany(p => p.BillDetails).HasForeignKey(p => p.ProductId);
        }
    }
}
