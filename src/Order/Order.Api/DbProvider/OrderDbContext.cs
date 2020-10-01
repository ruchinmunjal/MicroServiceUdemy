using Microsoft.EntityFrameworkCore;
using Order.Api.Entities;

namespace Order.Api.DbProvider
{
    
    public class OrderDbContext:DbContext
    {

        public OrderDbContext(DbContextOptions options):base(options)
        {
            
        }
        
        // The name of these properties will decide the name of the table which is used in the query generated SQL
        public DbSet<Entities.Order> Orders { get; set; } 
        
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShippingDetail> ShippingDetails { get; set; }
        
        
    }
}