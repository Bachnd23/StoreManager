using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace COCOApp.Repositories
{
    public class ReportsOrdersMappingRepository : IReportsOrdersMappingRepository
    {
        private readonly StoreManagerContext _context;

        public ReportsOrdersMappingRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public List<ReportsOrdersMapping> GetReportsOrdersMapping()
        {
            var query = _context.ReportsOrdersMappings.AsQueryable();
            query = query.Include(rom => rom.Order);
            return query.ToList();
        }
    }
}
