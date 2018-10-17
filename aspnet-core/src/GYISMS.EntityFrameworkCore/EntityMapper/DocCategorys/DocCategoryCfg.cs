

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GYISMS.DocCategories;

namespace GYISMS.EntityMapper.DocCategorys
{
    public class DocCategoryCfg : IEntityTypeConfiguration<DocCategory>
    {
        public void Configure(EntityTypeBuilder<DocCategory> builder)
        {

            builder.ToTable("DocCategorys", YoYoAbpefCoreConsts.SchemaNames.CMS);

            
			builder.Property(a => a.Name).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.ParentId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
			builder.Property(a => a.Desc).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


        }
    }
}


