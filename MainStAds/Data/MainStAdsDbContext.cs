using System.Collections.Generic;
using MainStAds.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MainStAds.Data
{
    public class MainStAdsDbContext : DbContext
    {
        public MainStAdsDbContext(DbContextOptions<MainStAdsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Business> Businesses { get; set; }
    }

}
