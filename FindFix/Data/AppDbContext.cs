using FindFix.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace FindFix.Data;
public class AppDbContext:IdentityDbContext <ApplicationUser>
{
    public AppDbContext (DbContextOptions <AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Technician> Technicians {get; set;}= null!;
    public DbSet<Customer> Customers {get; set;}= null!;
    public DbSet<Service> Services {get; set;}= null!;
    public DbSet<Qualification> Qualifications {get; set;}= null!;
    public DbSet<JobRequest> JobRequests {get; set;}= null!;
    public DbSet<Review> Reviews {get; set;}= null!;

}