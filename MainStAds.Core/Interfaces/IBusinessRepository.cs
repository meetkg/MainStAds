using System.Threading.Tasks;
using MainStAds.Core.Entities;
using MainStAds.Core.Repositories;

namespace MainStAds.Core.Interfaces
{
    public interface IBusinessRepository : IRepository<Business>
    {
        Task<bool> BusinessExistsAsync(int id);
    }
}
