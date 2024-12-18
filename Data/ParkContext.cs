using Microsoft.EntityFrameworkCore;
using NationalParksApi.Models;

namespace NationalParksApi.Data
{
    public class ParkContext : DbContext
    {
        public ParkContext(DbContextOptions<ParkContext> options) : base(options)
        {
        }

        public DbSet<NationalPark> Parks { get; set; } = null!;
    }
}
