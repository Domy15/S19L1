using Microsoft.EntityFrameworkCore;
using S19L1.Models;

namespace S19L1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentProfile> StudentProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentProfile>()
                .HasOne(sp => sp.Student)
                .WithOne(s => s.StudentProfile)
                .HasForeignKey<StudentProfile>(sp => sp.StudentId);
        }
    }
}
