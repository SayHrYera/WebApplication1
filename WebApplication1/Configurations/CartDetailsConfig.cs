using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Configurations
{
    public class CartDetailsConfig : IEntityTypeConfiguration<CartDetails>
    {
        public void Configure(EntityTypeBuilder<CartDetails> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Cart).WithMany(p => p.Details).HasForeignKey(p => p.Username);
            builder.HasOne(p => p.Product).WithMany(p => p.CartDetails).HasForeignKey(p => p.ProductId);
        }
    }
}
