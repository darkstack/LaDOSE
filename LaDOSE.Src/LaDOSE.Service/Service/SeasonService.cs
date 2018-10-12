using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;

namespace LaDOSE.Business.Service
{
    public class SeasonService : BaseService<Season>, ISeasonService
    {
        public SeasonService(LaDOSEDbContext context) : base(context)
        {
        }
    }
}