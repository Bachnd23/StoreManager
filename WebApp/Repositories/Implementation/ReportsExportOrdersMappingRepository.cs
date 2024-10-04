using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace COCOApp.Repositories
{
    public class ReportsExportOrdersMappingRepository : IReportsExportOrdersMappingRepository
    {
        private readonly StoreManagerContext _context;

        public ReportsExportOrdersMappingRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public List<ReportsExportOrdersMapping> GetReportsOrdersMapping()
        {
            var query = _context.ReportsExportOrdersMappings.AsQueryable();
            query = query.Include(rom => rom.Order);
            return query.ToList();
        }
    }
}
