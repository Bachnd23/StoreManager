using COCOApp.Models;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IReportsExportOrdersMappingRepository
    {
        List<ReportsExportOrdersMapping> GetReportsOrdersMapping();
    }
}
