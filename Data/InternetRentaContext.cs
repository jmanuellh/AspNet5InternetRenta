using Microsoft.EntityFrameworkCore;
using AspNet5InternetRenta.Models;

namespace AspNet5InternetRenta.Data
{
    public class InternetRentaContext : DbContext
    {
        public InternetRentaContext (DbContextOptions<InternetRentaContext> options)
            : base(options)
        {
        }

        public DbSet<InternetRenta> InternetRentas { get; set; }
    }
}
