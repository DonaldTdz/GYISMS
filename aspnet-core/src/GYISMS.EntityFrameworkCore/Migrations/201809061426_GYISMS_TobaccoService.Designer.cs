using System;
using GYISMS.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GYISMS.Migrations
{
    [DbContext(typeof(GYISMSDbContext))]
    [Migration("201809061426_GYISMS_TobaccoService")]
    partial class GYISMS_TobaccoService
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Abp.Application.Editions.Edition", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(32);

                b.HasKey("Id");

                b.ToTable("AbpEditions");
            });

            modelBuilder.Entity("Abp.Application.Features.FeatureSetting", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Discriminator")
                    .IsRequired();

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<int?>("TenantId");

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.ToTable("AbpFeatures");

                b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
            });

            modelBuilder.Entity("Abp.Auditing.AuditLog", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("BrowserInfo")
                    .HasMaxLength(512);

                b.Property<string>("ClientIpAddress")
                    .HasMaxLength(64);

                b.Property<string>("ClientName")
                    .HasMaxLength(128);

                b.Property<string>("CustomData")
                    .HasMaxLength(2000);

                b.Property<string>("Exception")
                    .HasMaxLength(2000);

                b.Property<int>("ExecutionDuration");

                b.Property<DateTime>("ExecutionTime");

                b.Property<int?>("ImpersonatorTenantId");

                b.Property<long?>("ImpersonatorUserId");

                b.Property<string>("MethodName")
                    .HasMaxLength(256);

                b.Property<string>("Parameters")
                    .HasMaxLength(1024);

                b.Property<string>("ServiceName")
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "ExecutionDuration");

                b.HasIndex("TenantId", "ExecutionTime");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpAuditLogs");
            });

            modelBuilder.Entity("Abp.Authorization.PermissionSetting", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Discriminator")
                    .IsRequired();

                b.Property<bool>("IsGranted");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpPermissions");

                b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RoleClaim", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType")
                    .HasMaxLength(256);

                b.Property<string>("ClaimValue");

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<int>("RoleId");

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.HasIndex("TenantId", "ClaimType");

                b.ToTable("AbpRoleClaims");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserAccount", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("EmailAddress")
                    .HasMaxLength(256);

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastLoginTime");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.Property<long?>("UserLinkId");

                b.Property<string>("UserName")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("EmailAddress");

                b.HasIndex("UserName");

                b.HasIndex("TenantId", "EmailAddress");

                b.HasIndex("TenantId", "UserId");

                b.HasIndex("TenantId", "UserName");

                b.ToTable("AbpUserAccounts");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserClaim", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType")
                    .HasMaxLength(256);

                b.Property<string>("ClaimValue");

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "ClaimType");

                b.ToTable("AbpUserClaims");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserLogin", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("LoginProvider")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<string>("ProviderKey")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "UserId");

                b.HasIndex("TenantId", "LoginProvider", "ProviderKey");

                b.ToTable("AbpUserLogins");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserLoginAttempt", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("BrowserInfo")
                    .HasMaxLength(512);

                b.Property<string>("ClientIpAddress")
                    .HasMaxLength(64);

                b.Property<string>("ClientName")
                    .HasMaxLength(128);

                b.Property<DateTime>("CreationTime");

                b.Property<byte>("Result");

                b.Property<string>("TenancyName")
                    .HasMaxLength(64);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.Property<string>("UserNameOrEmailAddress")
                    .HasMaxLength(255);

                b.HasKey("Id");

                b.HasIndex("UserId", "TenantId");

                b.HasIndex("TenancyName", "UserNameOrEmailAddress", "Result");

                b.ToTable("AbpUserLoginAttempts");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserOrganizationUnit", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<bool>("IsDeleted");

                b.Property<long>("OrganizationUnitId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "OrganizationUnitId");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpUserOrganizationUnits");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserRole", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<int>("RoleId");

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "RoleId");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpUserRoles");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserToken", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("LoginProvider")
                    .HasMaxLength(128);

                b.Property<string>("Name")
                    .HasMaxLength(128);

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.Property<string>("Value")
                    .HasMaxLength(512);

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpUserTokens");
            });

            modelBuilder.Entity("Abp.BackgroundJobs.BackgroundJobInfo", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<bool>("IsAbandoned");

                b.Property<string>("JobArgs")
                    .IsRequired()
                    .HasMaxLength(1048576);

                b.Property<string>("JobType")
                    .IsRequired()
                    .HasMaxLength(512);

                b.Property<DateTime?>("LastTryTime");

                b.Property<DateTime>("NextTryTime");

                b.Property<byte>("Priority");

                b.Property<short>("TryCount");

                b.HasKey("Id");

                b.HasIndex("IsAbandoned", "NextTryTime");

                b.ToTable("AbpBackgroundJobs");
            });

            modelBuilder.Entity("Abp.Configuration.Setting", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.Property<string>("Value")
                    .HasMaxLength(2000);

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpSettings");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityChange", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("ChangeTime");

                b.Property<byte>("ChangeType");

                b.Property<long>("EntityChangeSetId");

                b.Property<string>("EntityId")
                    .HasMaxLength(48);

                b.Property<string>("EntityTypeFullName")
                    .HasMaxLength(192);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("EntityChangeSetId");

                b.HasIndex("EntityTypeFullName", "EntityId");

                b.ToTable("AbpEntityChanges");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityChangeSet", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("BrowserInfo")
                    .HasMaxLength(512);

                b.Property<string>("ClientIpAddress")
                    .HasMaxLength(64);

                b.Property<string>("ClientName")
                    .HasMaxLength(128);

                b.Property<DateTime>("CreationTime");

                b.Property<string>("ExtensionData");

                b.Property<int?>("ImpersonatorTenantId");

                b.Property<long?>("ImpersonatorUserId");

                b.Property<string>("Reason")
                    .HasMaxLength(256);

                b.Property<int?>("TenantId");

                b.Property<long?>("UserId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "CreationTime");

                b.HasIndex("TenantId", "Reason");

                b.HasIndex("TenantId", "UserId");

                b.ToTable("AbpEntityChangeSets");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityPropertyChange", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<long>("EntityChangeId");

                b.Property<string>("NewValue")
                    .HasMaxLength(512);

                b.Property<string>("OriginalValue")
                    .HasMaxLength(512);

                b.Property<string>("PropertyName")
                    .HasMaxLength(96);

                b.Property<string>("PropertyTypeFullName")
                    .HasMaxLength(192);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("EntityChangeId");

                b.ToTable("AbpEntityPropertyChanges");
            });

            modelBuilder.Entity("Abp.Localization.ApplicationLanguage", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.Property<string>("Icon")
                    .HasMaxLength(128);

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsDisabled");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpLanguages");
            });

            modelBuilder.Entity("Abp.Localization.ApplicationLanguageText", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Key")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<string>("LanguageName")
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Source")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<int?>("TenantId");

                b.Property<string>("Value")
                    .IsRequired()
                    .HasMaxLength(67108864);

                b.HasKey("Id");

                b.HasIndex("TenantId", "Source", "LanguageName", "Key");

                b.ToTable("AbpLanguageTexts");
            });

            modelBuilder.Entity("Abp.Notifications.NotificationInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Data")
                    .HasMaxLength(1048576);

                b.Property<string>("DataTypeName")
                    .HasMaxLength(512);

                b.Property<string>("EntityId")
                    .HasMaxLength(96);

                b.Property<string>("EntityTypeAssemblyQualifiedName")
                    .HasMaxLength(512);

                b.Property<string>("EntityTypeName")
                    .HasMaxLength(250);

                b.Property<string>("ExcludedUserIds")
                    .HasMaxLength(131072);

                b.Property<string>("NotificationName")
                    .IsRequired()
                    .HasMaxLength(96);

                b.Property<byte>("Severity");

                b.Property<string>("TenantIds")
                    .HasMaxLength(131072);

                b.Property<string>("UserIds")
                    .HasMaxLength(131072);

                b.HasKey("Id");

                b.ToTable("AbpNotifications");
            });

            modelBuilder.Entity("Abp.Notifications.NotificationSubscriptionInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("EntityId")
                    .HasMaxLength(96);

                b.Property<string>("EntityTypeAssemblyQualifiedName")
                    .HasMaxLength(512);

                b.Property<string>("EntityTypeName")
                    .HasMaxLength(250);

                b.Property<string>("NotificationName")
                    .HasMaxLength(96);

                b.Property<int?>("TenantId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("NotificationName", "EntityTypeName", "EntityId", "UserId");

                b.HasIndex("TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId");

                b.ToTable("AbpNotificationSubscriptions");
            });

            modelBuilder.Entity("Abp.Notifications.TenantNotificationInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<string>("Data")
                    .HasMaxLength(1048576);

                b.Property<string>("DataTypeName")
                    .HasMaxLength(512);

                b.Property<string>("EntityId")
                    .HasMaxLength(96);

                b.Property<string>("EntityTypeAssemblyQualifiedName")
                    .HasMaxLength(512);

                b.Property<string>("EntityTypeName")
                    .HasMaxLength(250);

                b.Property<string>("NotificationName")
                    .IsRequired()
                    .HasMaxLength(96);

                b.Property<byte>("Severity");

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("TenantId");

                b.ToTable("AbpTenantNotifications");
            });

            modelBuilder.Entity("Abp.Notifications.UserNotificationInfo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreationTime");

                b.Property<int>("State");

                b.Property<int?>("TenantId");

                b.Property<Guid>("TenantNotificationId");

                b.Property<long>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId", "State", "CreationTime");

                b.ToTable("AbpUserNotifications");
            });

            modelBuilder.Entity("Abp.Organizations.OrganizationUnit", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Code")
                    .IsRequired()
                    .HasMaxLength(95);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<long?>("ParentId");

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("ParentId");

                b.HasIndex("TenantId", "Code");

                b.ToTable("AbpOrganizationUnits");
            });

            modelBuilder.Entity("GYISMS.Authorization.Roles.Role", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasMaxLength(128);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("Description")
                    .HasMaxLength(5000);

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.Property<bool>("IsDefault");

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsStatic");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<string>("NormalizedName")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<int?>("TenantId");

                b.HasKey("Id");

                b.HasIndex("CreatorUserId");

                b.HasIndex("DeleterUserId");

                b.HasIndex("LastModifierUserId");

                b.HasIndex("TenantId", "NormalizedName");

                b.ToTable("AbpRoles");
            });

            modelBuilder.Entity("GYISMS.Authorization.Users.User", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<int>("AccessFailedCount");

                b.Property<string>("AuthenticationSource")
                    .HasMaxLength(64);

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasMaxLength(128);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<string>("EmailAddress")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<string>("EmailConfirmationCode")
                    .HasMaxLength(328);

                b.Property<bool>("IsActive");

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsEmailConfirmed");

                b.Property<bool>("IsLockoutEnabled");

                b.Property<bool>("IsPhoneNumberConfirmed");

                b.Property<bool>("IsTwoFactorEnabled");

                b.Property<DateTime?>("LastLoginTime");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<DateTime?>("LockoutEndDateUtc");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<string>("NormalizedEmailAddress")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .IsRequired()
                    .HasMaxLength(256);

                b.Property<string>("Password")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<string>("PasswordResetCode")
                    .HasMaxLength(328);

                b.Property<string>("PhoneNumber")
                    .HasMaxLength(32);

                b.Property<string>("SecurityStamp")
                    .HasMaxLength(128);

                b.Property<string>("Surname")
                    .IsRequired()
                    .HasMaxLength(32);

                b.Property<int?>("TenantId");

                b.Property<string>("UserName")
                    .IsRequired()
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("CreatorUserId");

                b.HasIndex("DeleterUserId");

                b.HasIndex("LastModifierUserId");

                b.HasIndex("TenantId", "NormalizedEmailAddress");

                b.HasIndex("TenantId", "NormalizedUserName");

                b.ToTable("AbpUsers");
            });

            modelBuilder.Entity("GYISMS.MultiTenancy.Tenant", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ConnectionString")
                    .HasMaxLength(1024);

                b.Property<DateTime>("CreationTime");

                b.Property<long?>("CreatorUserId");

                b.Property<long?>("DeleterUserId");

                b.Property<DateTime?>("DeletionTime");

                b.Property<int?>("EditionId");

                b.Property<bool>("IsActive");

                b.Property<bool>("IsDeleted");

                b.Property<DateTime?>("LastModificationTime");

                b.Property<long?>("LastModifierUserId");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(128);

                b.Property<string>("TenancyName")
                    .IsRequired()
                    .HasMaxLength(64);

                b.HasKey("Id");

                b.HasIndex("CreatorUserId");

                b.HasIndex("DeleterUserId");

                b.HasIndex("EditionId");

                b.HasIndex("LastModifierUserId");

                b.HasIndex("TenancyName");

                b.ToTable("AbpTenants");
            });

            modelBuilder.Entity("Abp.Application.Features.EditionFeatureSetting", b =>
            {
                b.HasBaseType("Abp.Application.Features.FeatureSetting");

                b.Property<int>("EditionId");

                b.HasIndex("EditionId", "Name");

                b.ToTable("AbpFeatures");

                b.HasDiscriminator().HasValue("EditionFeatureSetting");
            });

            modelBuilder.Entity("Abp.MultiTenancy.TenantFeatureSetting", b =>
            {
                b.HasBaseType("Abp.Application.Features.FeatureSetting");


                b.HasIndex("TenantId", "Name");

                b.ToTable("AbpFeatures");

                b.HasDiscriminator().HasValue("TenantFeatureSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RolePermissionSetting", b =>
            {
                b.HasBaseType("Abp.Authorization.PermissionSetting");

                b.Property<int>("RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AbpPermissions");

                b.HasDiscriminator().HasValue("RolePermissionSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserPermissionSetting", b =>
            {
                b.HasBaseType("Abp.Authorization.PermissionSetting");

                b.Property<long>("UserId");

                b.HasIndex("UserId");

                b.ToTable("AbpPermissions");

                b.HasDiscriminator().HasValue("UserPermissionSetting");
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RoleClaim", b =>
            {
                b.HasOne("GYISMS.Authorization.Roles.Role")
                    .WithMany("Claims")
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserClaim", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User")
                    .WithMany("Claims")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserLogin", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User")
                    .WithMany("Logins")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserRole", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User")
                    .WithMany("Roles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserToken", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User")
                    .WithMany("Tokens")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Configuration.Setting", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User")
                    .WithMany("Settings")
                    .HasForeignKey("UserId");
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityChange", b =>
            {
                b.HasOne("Abp.EntityHistory.EntityChangeSet")
                    .WithMany("EntityChanges")
                    .HasForeignKey("EntityChangeSetId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.EntityHistory.EntityPropertyChange", b =>
            {
                b.HasOne("Abp.EntityHistory.EntityChange")
                    .WithMany("PropertyChanges")
                    .HasForeignKey("EntityChangeId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Organizations.OrganizationUnit", b =>
            {
                b.HasOne("Abp.Organizations.OrganizationUnit", "Parent")
                    .WithMany("Children")
                    .HasForeignKey("ParentId");
            });

            modelBuilder.Entity("GYISMS.Authorization.Roles.Role", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User", "CreatorUser")
                    .WithMany()
                    .HasForeignKey("CreatorUserId");

                b.HasOne("GYISMS.Authorization.Users.User", "DeleterUser")
                    .WithMany()
                    .HasForeignKey("DeleterUserId");

                b.HasOne("GYISMS.Authorization.Users.User", "LastModifierUser")
                    .WithMany()
                    .HasForeignKey("LastModifierUserId");
            });

            modelBuilder.Entity("GYISMS.Authorization.Users.User", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User", "CreatorUser")
                    .WithMany()
                    .HasForeignKey("CreatorUserId");

                b.HasOne("GYISMS.Authorization.Users.User", "DeleterUser")
                    .WithMany()
                    .HasForeignKey("DeleterUserId");

                b.HasOne("GYISMS.Authorization.Users.User", "LastModifierUser")
                    .WithMany()
                    .HasForeignKey("LastModifierUserId");
            });

            modelBuilder.Entity("GYISMS.MultiTenancy.Tenant", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User", "CreatorUser")
                    .WithMany()
                    .HasForeignKey("CreatorUserId");

                b.HasOne("GYISMS.Authorization.Users.User", "DeleterUser")
                    .WithMany()
                    .HasForeignKey("DeleterUserId");

                b.HasOne("Abp.Application.Editions.Edition", "Edition")
                    .WithMany()
                    .HasForeignKey("EditionId");

                b.HasOne("GYISMS.Authorization.Users.User", "LastModifierUser")
                    .WithMany()
                    .HasForeignKey("LastModifierUserId");
            });

            modelBuilder.Entity("Abp.Application.Features.EditionFeatureSetting", b =>
            {
                b.HasOne("Abp.Application.Editions.Edition", "Edition")
                    .WithMany()
                    .HasForeignKey("EditionId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Roles.RolePermissionSetting", b =>
            {
                b.HasOne("GYISMS.Authorization.Roles.Role")
                    .WithMany("Permissions")
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Abp.Authorization.Users.UserPermissionSetting", b =>
            {
                b.HasOne("GYISMS.Authorization.Users.User")
                    .WithMany("Permissions")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618

            modelBuilder.Entity("GYISMS.Organizations.Organization", b =>
            {
                b.Property<long>("Id").ValueGeneratedOnAdd();
                //.ValueGeneratedOnAdd()
                //.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
                b.Property<string>("DepartmentName").IsRequired().HasMaxLength(100);
                b.Property<long?>("ParentId");
                b.Property<long?>("Order");
                b.Property<bool?>("DeptHiding");
                b.Property<string>("OrgDeptOwner").HasMaxLength(100);
                b.Property<DateTime?>("CreationTime");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Organizations");
            });

            modelBuilder.Entity("GYISMS.Employees.Employee", b =>
            {
                b.Property<string>("Id").ValueGeneratedOnAdd();
                b.Property<string>("OpenId").HasMaxLength(200);
                b.Property<string>("Name").HasMaxLength(50);
                b.Property<string>("Mobile").HasMaxLength(11);
                b.Property<string>("Email").HasMaxLength(100);
                b.Property<bool?>("Active");
                b.Property<bool?>("IsAdmin");
                b.Property<bool?>("IsBoss");
                b.Property<string>("Department").HasMaxLength(300);
                b.Property<string>("Position").HasMaxLength(100);
                b.Property<string>("Avatar").HasMaxLength(200);
                b.Property<string>("HiredDate").HasMaxLength(100);
                b.Property<string>("Roles").HasMaxLength(300);
                b.Property<long?>("RoleId");
                b.Property<string>("Remark").HasMaxLength(500);
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Employees");
            });

            modelBuilder.Entity("GYISMS.SystemDatas.SystemData", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<int?>("ModelId");
                b.Property<int>("Type").IsRequired();
                b.Property<string>("Code").IsRequired().HasMaxLength(50);
                b.Property<string>("Desc").IsRequired().HasMaxLength(500);
                b.Property<int?>("Seq");
                b.Property<DateTime?>("CreationTime");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("SystemDatas");
            });
            modelBuilder.Entity("GYISMS.Meetings.Meeting", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<int>("MeetingRoomId").IsRequired();
                b.Property<string>("Subject").IsRequired().HasMaxLength(100);
                b.Property<string>("Issues").HasMaxLength(2000);
                b.Property<string>("Desc").HasMaxLength(500);
                b.Property<DateTime>("BeginTime").IsRequired();
                b.Property<DateTime>("EndTime").IsRequired();
                b.Property<string>("HostId").HasMaxLength(100);
                b.Property<string>("HostName");
                b.Property<int?>("NoticeWay");
                b.Property<int?>("RemindingWay");
                b.Property<int?>("RemindingTime");
                b.Property<int?>("Status");
                b.Property<string>("AuditId").HasMaxLength(100);
                b.Property<string>("AuditName");
                b.Property<DateTime?>("AuditTime");
                b.Property<string>("CancelUserId").HasMaxLength(100);
                b.Property<string>("CancelUserName").HasMaxLength(50);
                b.Property<DateTime?>("CancelTime");
                b.Property<string>("ResponsibleId").HasMaxLength(100);
                b.Property<string>("ResponsibleName");
                b.Property<bool?>("IsSeatingOrder");
                b.Property<string>("Summary");
                b.Property<string>("FilePath").HasMaxLength(500);
                b.Property<bool?>("IsDeleted");
                b.Property<DateTime?>("CreationTime");
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Meetings");
            });

            modelBuilder.Entity("GYISMS.MeetingMaterials.MeetingMaterial", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("MeetingId").IsRequired();
                b.Property<string>("Code");
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<int?>("Num");
                b.Property<DateTime?>("CreationTime");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("MeetingMaterials");
            });

            modelBuilder.Entity("GYISMS.MeetingParticipants.MeetingParticipant", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("MeetingId").IsRequired();
                b.Property<string>("UserId").HasMaxLength(100);
                b.Property<string>("UserName").IsRequired();
                b.Property<int?>("Row");
                b.Property<int?>("Column");
                b.Property<DateTime?>("ConfirmTime");
                b.Property<DateTime?>("SignTime");
                b.Property<DateTime?>("CreationTime");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("MeetingParticipants");
            });

            modelBuilder.Entity("GYISMS.MeetingRooms.MeetingRoom ", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<string>("Photo").IsRequired().HasMaxLength(200);
                b.Property<int>("Num").IsRequired();
                b.Property<int?>("RoomType");
                b.Property<string>("Address").HasMaxLength(500);
                b.Property<string>("BuildDesc");
                b.Property<bool?>("IsApprove");
                b.Property<string>("ManagerId").HasMaxLength(100);
                b.Property<string>("ManagerName").HasMaxLength(50);
                b.Property<int?>("Row");
                b.Property<int?>("Column");
                b.Property<int?>("LayoutPattern");
                b.Property<string>("PlanPath");
                b.Property<string>("Remark").HasMaxLength(500);
                b.Property<string>("Devices").HasMaxLength(500);
                b.Property<bool?>("IsDeleted");
                b.Property<DateTime?>("CreationTime");
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("MeetingRooms");
            });

            modelBuilder.Entity("HC.WeChat.Growers.Grower", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<int?>("Year");
                b.Property<string>("UnitCode").HasMaxLength(20);
                b.Property<string>("UnitName").HasMaxLength(50);
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<int?>("CountyCode");
                b.Property<string>("EmployeeId").HasMaxLength(200);
                b.Property<string>("EmployeeName").HasMaxLength(200);
                b.Property<string>("ContractNo").HasMaxLength(50);
                b.Property<string>("VillageGroup").HasMaxLength(50);
                b.Property<string>("Tel").HasMaxLength(20);
                b.Property<string>("Address").HasMaxLength(500);
                b.Property<int?>("Type");
                b.Property<decimal?>("PlantingArea");
                b.Property<decimal?>("Longitude");
                b.Property<decimal?>("Latitude");
                b.Property<DateTime?>("ContractTime");
                b.Property<bool?>("IsDeleted");
                b.Property<DateTime?>("CreationTime");
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Growers");
            });        

            modelBuilder.Entity("GYISMS.Schedules.Schedule", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Desc").HasMaxLength(500);
                b.Property<int>("Type").IsRequired();
                b.Property<DateTime?>("BeginTime");
                b.Property<DateTime?>("EndTime");
                b.Property<int?>("Status");
                b.Property<DateTime?>("PublishTime");
                b.Property<bool?>("IsDeleted");
                b.Property<DateTime?>("CreationTime");
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.Property<string>("Name").IsRequired().HasMaxLength(200);
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("Schedules");
            });

            modelBuilder.Entity("GYISMS.ScheduleDetails.ScheduleDetail", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<int>("TaskId").IsRequired();
                b.Property<Guid>("ScheduleId").IsRequired();
                b.Property<string>("EmployeeId").IsRequired().HasMaxLength(50);
                b.Property<int>("GrowerId").IsRequired();
                b.Property<int?>("VisitNum");
                b.Property<int?>("CompleteNum");
                b.Property<DateTime?>("CreationTime");
                b.Property<int?>("Status");
                b.Property<Guid>("ScheduleTaskId");
                b.Property<string>("EmployeeName").HasMaxLength(50);
                b.Property<string>("GrowerName").HasMaxLength(50);
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ScheduleDetails");
            });

            modelBuilder.Entity("GYISMS.ScheduleTasks.ScheduleTask", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<int>("TaskId").IsRequired();
                b.Property<string>("TaskName").HasMaxLength(200);
                b.Property<Guid>("ScheduleId").IsRequired();
                b.Property<int?>("VisitNum");
                b.Property<bool?>("IsDeleted");
                b.Property<DateTime?>("CreationTime");
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");

                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("ScheduleTasks");
            });

            modelBuilder.Entity("GYISMS.VisitTasks.VisitTask", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<int>("Type").IsRequired();
                b.Property<bool?>("IsExamine");
                b.Property<string>("Desc").HasMaxLength(500);
                b.Property<bool?>("IsDeleted");
                b.Property<DateTime?>("CreationTime");
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("VisitTasks");
            });

            modelBuilder.Entity("GYISMS.TaskExamines.TaskExamine", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd();
                b.Property<int?>("TaskId");
                b.Property<string>("Name").IsRequired().HasMaxLength(50);
                b.Property<string>("Desc").HasMaxLength(500);
                b.Property<int?>("Seq");
                b.Property<DateTime?>("CreationTime");
                b.Property<bool?>("IsDeleted");
                b.Property<long?>("CreatorUserId");
                b.Property<DateTime?>("LastModificationTime");
                b.Property<long?>("LastModifierUserId");
                b.Property<DateTime?>("DeletionTime");
                b.Property<long?>("DeleterUserId");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("TaskExamines");
            });

            modelBuilder.Entity("GYISMS.VisitExamines.VisitExamine", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid?>("VisitRecordId");
                b.Property<string>("EmployeeId").HasMaxLength(200);
                b.Property<int?>("GrowerId");
                b.Property<int?>("TaskExamineId");
                b.Property<int?>("Score");
                b.Property<DateTime?>("CreationTime");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("VisitExamines");
            });

            modelBuilder.Entity("GYISMS.VisitRecords.VisitRecord", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<Guid>("ScheduleDetailId").IsRequired();
                b.Property<string>("EmployeeId").HasMaxLength(200);
                b.Property<int?>("GrowerId");
                b.Property<DateTime?>("SignTime");
                b.Property<string>("Location").HasMaxLength(200);
                b.Property<decimal?>("Longitude");
                b.Property<decimal?>("Latitude");
                b.Property<string>("Desc").HasMaxLength(500);
                b.Property<string>("ImgPath").HasMaxLength(200);
                b.Property<DateTime?>("CreationTime");
                b.HasKey("Id");

                //b.HasIndex("TargetTenantId", "TargetUserId", "ReadState");

                b.ToTable("VisitRecords");
            });
        }
    }
}
