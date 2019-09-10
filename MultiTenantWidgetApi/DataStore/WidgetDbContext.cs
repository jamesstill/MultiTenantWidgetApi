using Microsoft.EntityFrameworkCore;
using MultiTenantWidgetApi.Models;

namespace MultiTenantWidgetApi.DataStore
{
    public partial class WidgetDbContext : DbContext
    {
        public WidgetDbContext(DbContextOptions<WidgetDbContext> options) : base(options) { }

        public virtual DbSet<Widget> Widgets { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new WidgetMap());
        }
    }
}
