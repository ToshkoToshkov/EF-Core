using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public StudentSystemContext()
        {
        }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Homework> HomeworkSubmissions { get; set; }

        public virtual DbSet<StudentCourse> StudentCourses { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>(e =>
            {
                e.HasKey(k => new { k.CourseId, k.StudentId });
            });

            
            modelBuilder.Entity<Student>().Property(e => e.PhoneNumber).HasMaxLength(10).IsFixedLength();
            
            modelBuilder.Entity<Student>().Property(e => e.PhoneNumber).IsUnicode(false);

            modelBuilder.Entity<Resource>().Property(e => e.Url).IsUnicode(false);

            modelBuilder.Entity<Homework>().Property(e => e.Content).IsUnicode(false);
        }
    }
}
