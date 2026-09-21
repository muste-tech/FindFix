using FindFix.Models;
using Microsoft.EntityFrameworkCore;
namespace FindFix.Data;
public class AppDbCOntext:DbContext
{
    public AppDbCOntext (DbContextOptions <AppDbCOntext> options) : base(options)
    {
        
    }
    public DbSet<Technician> Technicians {get; set;}= null!;
    public DbSet<Customer> Customers {get; set;}= null!;
    public DbSet<Service> Services {get; set;}= null!;
    public DbSet<Qualification> Qualifications {get; set;}= null!;
    public DbSet<JobRequest> JobRequests {get; set;}= null!;
    public DbSet<Review> Reviews {get; set;}= null!;

}