using MainStAds.Core.Entities;
using MainStAds.Core.Interfaces;
using MainStAds.Core.Repositories;
using MainStAds.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MainStAds.Infrastructure.Repositories
{
    public class BusinessRepository : Repository<Business>, IBusinessRepository
    {
        private readonly MainStAdsDbContext _context;

        public BusinessRepository(MainStAdsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> BusinessExistsAsync(int id)
        {
            return await _context.Businesses.AnyAsync(e => e.Id == id);
        }
    }
}
