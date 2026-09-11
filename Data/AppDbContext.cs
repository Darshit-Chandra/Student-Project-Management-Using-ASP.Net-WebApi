using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Data
{
    public class AppDbContext:DbContext
    {
	public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
    
        public DbSet<RoleModel> Role => Set<RoleModel>();
        public DbSet<TaskModel> Task    => Set<TaskModel>();
        public DbSet<ProjectAllocationModel> ProjectAllocation => Set<ProjectAllocationModel>();
        public DbSet<ProjectMasterModel> projectMaster => Set<ProjectMasterModel>();
        public DbSet<TaskPriorityModel>  TaskPriority => Set<TaskPriorityModel>();
        public DbSet<TaskStatusModel> TaskStatus => Set<TaskStatusModel>();
        public DbSet<UserModel> User => Set<UserModel>();
        public DbSet<UserRoleModel> UserRoles => Set<UserRoleModel>();
        public DbSet <UserTypeModel> UserTypes => Set<UserTypeModel>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // RoleName must be unique
            modelBuilder.Entity<RoleModel>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            // Email must be unique
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // ProjectAllocation -> Student
            modelBuilder.Entity<ProjectAllocationModel>()
                .HasOne(p => p.Student)
                .WithMany(u => u.StudentProjectAllocations)
                .HasForeignKey(p => p.StudentID)
                .OnDelete(DeleteBehavior.NoAction);


            // ProjectAllocation -> Faculty
            modelBuilder.Entity<ProjectAllocationModel>()
                .HasOne(p => p.Faculty)
                .WithMany(u => u.FacultyProjectAllocations)
                .HasForeignKey(p => p.FacultyID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
