using Microsoft.EntityFrameworkCore;
using Domain.Models;
namespace StudyProj
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Facility> Facilities { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Chief> Chiefs { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Discipline> Disciplines { get; set; }

        public DbSet<Semestr> Semesters { get; set; }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Attendance> Attendance { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany() 
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Schedule)
                .WithMany()  
                .HasForeignKey(a => a.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);  

            base.OnModelCreating(modelBuilder);
        }


    }
}
