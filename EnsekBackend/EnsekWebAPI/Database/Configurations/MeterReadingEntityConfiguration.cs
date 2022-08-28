using EnsekWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnsekWebAPI.Database.Configurations
{
  public class MeterReadingEntityConfiguration : IEntityTypeConfiguration<MeterReadingEntity>
  {
    public void Configure(EntityTypeBuilder<MeterReadingEntity> builder)
    {
      builder.ToTable("METER_READINGS");

      builder.HasKey(f => f.MeterReadingId);

      builder.Property(f => f.MeterReadingId)
             .HasColumnName("METER_READING_ID")
             .ValueGeneratedOnAdd();

      builder.Property(f => f.AccountId)
             .HasColumnName("ACCOUNT_ID")
             .IsRequired();

      builder.Property(f => f.MeterReadingDateTime)
             .HasColumnName("METER_READING_DATE")
             .IsRequired();

      builder.Property(f => f.MeterReadValue)
             .HasColumnName("METER_READING_VALUE")
             .IsRequired();
    }

  }
}
