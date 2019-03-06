

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GYISMS.Advises;

namespace GYISMS.EntityMapper.Advises
{
    public class AdviseCfg : IEntityTypeConfiguration<Advise>
    {
        public void Configure(EntityTypeBuilder<Advise> builder)
        {

            builder.ToTable("Advises", YoYoAbpefCoreConsts.SchemaNames.CMS);

            
			builder.Property(a => a.DocumentId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.EmployeeId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.EmployeeName).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.Content).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.CreationTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


