using Microsoft.EntityFrameworkCore;
using FitMatch.Models;

namespace FitMatch.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> users {get;set;}
        public DbSet<Post> posts {get;set;}
    }
}