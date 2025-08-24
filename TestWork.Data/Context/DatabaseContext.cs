using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestWork.Data.Entities;
using TestWork.Entities;

namespace TestWork.Data.Context;

public class DatabaseContext : DbContext
{
    internal const string SchemaName = "testwork";

    internal DbSet<UserEntity> Users { get; set; }
    internal DbSet<ProjectEntity> Projects { get; set; }
    internal DbSet<ProjectStageEntity> Stages { get; set; }
    internal DbSet<ProjectTaskEntity> Tasks { get; set; }


    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
         // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);

        BuildUsers(modelBuilder);
        BuildProjects(modelBuilder);
        BuildProjectStages(modelBuilder);
        BuildProjectTasks(modelBuilder);

        modelBuilder.Entity<UserEntity>().HasData([
            new UserEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Lol",
                Email = "admin@gmail.com",
                PasswordHash = "admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTimeOffset.Now
            }
        ]);
    }

    private static void BuildUsers(ModelBuilder builder)
    {
        builder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Email).IsRequired().HasMaxLength(128);
            entity.Property(o => o.FirstName).HasMaxLength(128);
            entity.Property(o => o.LastName).HasMaxLength(128);
            entity.Property(o => o.MiddleName).HasMaxLength(128);
            entity.Property(o => o.PasswordHash).HasMaxLength(1024);
            entity.ToTable("users");
        });
    }

    private static void BuildProjects(ModelBuilder builder)
    {
        builder.Entity<ProjectEntity>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Status).HasConversion<EnumToStringConverter<ProjectStatus>>();
            entity.HasMany(o => o.Tasks)
                .WithOne()
                .HasForeignKey(o => o.ProjectId);
            entity.HasMany(o => o.Stages)
                .WithOne()
                .HasForeignKey(o => o.ProjectId);
            entity.ToTable("projects");
        });
    }

    private static void BuildProjectTasks(ModelBuilder builder)
    {
        builder.Entity<ProjectTaskEntity>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.ToTable("tasks");
        });
    }

    private static void BuildProjectStages(ModelBuilder builder)
    {
        builder.Entity<ProjectStageEntity>(entity =>
        {
            entity.HasKey(o => o.Id);
            // entity.HasIndex(o => new { o.Stage, o.ProjectId }).IsUnique();

            entity.ToTable("stages");
        });
    }
}