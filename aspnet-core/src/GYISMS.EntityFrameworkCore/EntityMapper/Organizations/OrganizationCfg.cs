


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GYISMS.Organizations;

namespace GYISMS.EntityMapper.Organizations
{
public class OrganizationCfg : IEntityTypeConfiguration<Organization>
	{
	public void Configure(EntityTypeBuilder<Organization>
		builder)
		{

		builder.ToTable("Organizations", YoYoAbpefCoreConsts.SchemaNames.CMS);

		builder.Property(a => a.DepartmentName).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.ParentId).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.Order).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.DeptHiding).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.OrgDeptOwner).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);
builder.Property(a => a.CreationTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length64);


		}
		}
		}


