using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lil.TimeTracker.Models;

public class TimeTrackerDbContext: IdentityDbContext<IdentityUser>
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