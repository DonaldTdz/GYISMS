


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GYISMS.Growers;

namespace GYISMS.EntityMapper.Growers
{
public class GrowerCfg : IEntityTypeConfiguration<Grower>
	{
	public void Configure(EntityTypeBuilder<Grower>
		builder)
		{

		builder.ToTable("Growers", YoYoAbpefCoreConsts.SchemaNames.CMS);

		builder.Property(a => a.Year).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.UnitCode).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.UnitName).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.Name).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.CountyCode).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.EmployeeId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.ContractNo).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.VillageGroup).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.Tel).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.Address).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.Type).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.PlantingArea).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.Longitude).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.Latitude).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.ContractTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.IsDeleted).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.CreationTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.CreatorUserId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.LastModificationTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.LastModifierUserId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.DeletionTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.DeleterUserId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


		}
		}
		}


