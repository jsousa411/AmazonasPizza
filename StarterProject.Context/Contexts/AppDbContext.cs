using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarterProject.Context.Base;
using StarterProject.Context.Contexts.AppContext;

namespace StarterProject.Context.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly bool _enableLazyLoad = true;

        public AppDbContext()
        {
        }

        public AppDbContext(bool enableLazyLoad)
        {
            _enableLazyLoad = enableLazyLoad;
        }

        public DbSet<User> User { get; set; }

        public DbSet<Products> Product { get; set; }

        public DbSet<AdvertisementConfig> AdvertisementConfig { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddConfiguration(new UserConfiguration());
            modelBuilder.AddConfiguration(new ProductNameConfiguration());
            modelBuilder.AddConfiguration(new AdvertisementConfigConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_enableLazyLoad)
                {
                    optionsBuilder.UseLazyLoadingProxies();
                }
                
                var config = new ConfigurationBuilder()
                   //.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("contextsettings.json", optional: false)
                   .Build();
                
                var connectionStrings = config.GetSection("ConnectionStrings").Get<Dictionary<string, string>>();

                optionsBuilder.UseSqlServer(connectionStrings["StarterProject"]);
            }
        }
    }
}