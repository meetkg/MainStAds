using MainStAds.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MainStAds.Infrastructure.Data
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
