using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.EntityModels;


namespace OrderApi.Infrastructure.Database
{
    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<OrderModel> Orders { get; set; }
    }
}
