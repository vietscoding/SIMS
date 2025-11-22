using Microsoft.EntityFrameworkCore;
using SIMS.Models;

namespace SIMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People => Set<Person>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<AcademicProgram> AcademicPrograms => Set<AcademicProgram>();
        public DbSet<Faculty> Faculties => Set<Faculty>();
        public DbSet<Major> Majors => Set<Major>();
        public DbSet<Course> Courses => Set<Course>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.PersonId);
                entity.Property(e => e.FullName).HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
                entity.Property(e => e.CitizenIdNumber).HasMaxLength(20);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);
                entity.Property(e => e.StudentCode).HasMaxLength(50);
                entity.Property(e => e.PhoneNumberOfRelatives).HasMaxLength(50);
                entity.HasOne(e => e.Person)
                      .WithOne(p => p.Student)
                      .HasForeignKey<Student>(e => e.PersonId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AcademicProgram>(entity =>
            {
                entity.HasKey(e => e.AcademicProgramId);
                entity.Property(e => e.AcademicProgramName).HasMaxLength(255);
                entity.Property(e => e.Language).HasMaxLength(100);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);
                entity.Property(e => e.CourseName).HasMaxLength(255);
                entity.Property(e => e.CourseCode).HasMaxLength(50);
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.HasKey(e => e.FacultyId);
                entity.Property(e => e.FacultyName).HasMaxLength(255);
                entity.Property(e => e.TenKhoa).HasMaxLength(255);
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.HasKey(e => e.MajorId);
                entity.Property(e => e.MajorName).HasMaxLength(255);
                entity.Property(e => e.AlternativeMajorName).HasMaxLength(255);
                entity.Property(e => e.TenNganh).HasMaxLength(255);
            });
        }
    }
}
