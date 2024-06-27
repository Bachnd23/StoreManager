using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace COCOApp.Services
{
    public class ReportsOrdersMappingService: StoreManagerService
    {
        public List<ReportsOrdersMapping> GetReportsOrdersMapping()
        {
            var query = _context.ReportsOrdersMappings.AsQueryable();
            query = query.Include(rom => rom.Order);
            return query.ToList();
        }

    }
}
