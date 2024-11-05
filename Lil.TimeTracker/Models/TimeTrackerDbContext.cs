using Microsoft.EntityFrameworkCore;

namespace Lil.TimeTracker.Models;

public class TimeTrackerDbContext: DbContext
{
    public TimeTrackerDbContext()
    {
    }

    public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options) : base(options)
    {}

    public DbSet<Employee> Employees{ get; set; }
    public DbSet<Project> Projects{ get; set; }
    public DbSet<TimeEntry> TimeEntries{ get; set; }
}