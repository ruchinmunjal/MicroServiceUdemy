using Microsoft.EntityFrameworkCore;
using Order.Api.Entities;

namespace Order.Api.DbProvider
{
    
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Entities.Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        
        
    }
}