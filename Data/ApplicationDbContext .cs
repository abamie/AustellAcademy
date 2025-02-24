using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AustellAcademyAdmissions.Models;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<ClassRoutine> ClassRoutines { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Photo> Photos { get; set; }

     public DbSet<Class> Classes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Ensure Identity models are configured

        modelBuilder.Entity<ClassRoutine>().HasData(
            new ClassRoutine { Id = 1, ClassName = "Nursery", Subject = "Mathematics", Teacher = "Mr. Sangita", DayOfWeek = "Monday", StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
            new ClassRoutine { Id = 2, ClassName = "Preparatory", Subject = "English", Teacher = "Ms. Bindya", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
            new ClassRoutine { Id = 3, ClassName = "Preparatory", Subject = "Mathematics", Teacher = "Ms. Johnson", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
            new ClassRoutine { Id = 4, ClassName = "Preparatory", Subject = "EVS", Teacher = "Ms. Johnson", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
            new ClassRoutine { Id = 5, ClassName = "Preparatory", Subject = "GK", Teacher = "Ms. Johnson", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
            new ClassRoutine { Id = 6, ClassName = "Nursery", Subject = "EVS", Teacher = "Ms. Johnson", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
            new ClassRoutine { Id = 7, ClassName = "Nursery", Subject = "GK", Teacher = "Ms. Johnson", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
            new ClassRoutine { Id = 8, ClassName = "Preparatory", Subject = "English", Teacher = "Mrs. Alpana", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
            new ClassRoutine { Id = 9, ClassName = "Preparatory", Subject = "Mathematics", Teacher = "Mrs. Pollabi", DayOfWeek = "Tuesday", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) }
        );
    }
}
