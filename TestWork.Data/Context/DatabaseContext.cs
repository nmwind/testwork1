using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestWork.Data.Entities;
using TestWork.Entities;
using TestWork.ReadModels;

namespace TestWork.Data.Context;

public class DatabaseContext : DbContext
{
    internal const string SchemaName = "testwork";

    internal DbSet<UserEntity> Users { get; set; }= null!;
    internal DbSet<ProjectEntity> Projects { get; set; }= null!;
    internal DbSet<ProjectStageEntity> Stages { get; set; } = null!;
    internal DbSet<ProjectTaskEntity> Tasks { get; set; }= null!;


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
                UpdatedAt = DateTime.UtcNow
            }
        ]);

        Guid projectId = Guid.NewGuid();
        modelBuilder.Entity<ProjectEntity>().HasData([
            new ProjectEntity
            {
                Id = projectId,
                Description = "",
                Risks = "",
                Name = "virtual project",
                Status = ProjectStatus.PreProject,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow
            }
        ]);

        modelBuilder.Entity<ProjectStageEntity>().HasData([
            new ProjectStageEntity
            {
                ProjectId = projectId,
                Stage = 0,
                Title = "stage 1",
            }
        ]);
        
        modelBuilder.Entity<ProjectTaskEntity>().HasData([
            new ProjectTaskEntity
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                Stage = 0,
                Order = 0,
                Title = "Task 1",
                Start = DateOnly.FromDateTime(DateTime.UtcNow),
                End = DateOnly.FromDateTime(DateTime.UtcNow),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
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
                .HasForeignKey(o => o.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            
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
            entity.HasKey(o => new { o.Stage, o.ProjectId });

            entity.ToTable("stages");
        });
    }
}