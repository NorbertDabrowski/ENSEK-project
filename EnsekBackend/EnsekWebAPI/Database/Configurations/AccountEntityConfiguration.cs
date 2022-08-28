using EnsekWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnsekWebAPI.Database.Configurations
{
  public class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
  {
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
      builder.ToTable("ACCOUNTS");

      builder.HasKey(f => f.AccountId);

      builder.Property(f => f.AccountId)
             .HasColumnName("ACCOUNT_ID");

      builder.Property(f => f.FirstName)
             .HasColumnName("FIRST_NAME")
             .IsRequired();

      builder.Property(f => f.LastName)
             .HasColumnName("LAST_NAME")
             .IsRequired();
    }

  }
}
