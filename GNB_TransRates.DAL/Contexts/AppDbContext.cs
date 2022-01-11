using GNB_TransRates.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GNB_TransRates.DAL.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> con) : base(con) { }

        public virtual DbSet<Rates> t_rates { get; set; }
        public virtual DbSet<Transactions> t_transactions { get; set; }
    }
}
