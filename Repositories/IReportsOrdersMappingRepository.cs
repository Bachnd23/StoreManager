using COCOApp.Models;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IReportsOrdersMappingRepository
    {
        List<ReportsOrdersMapping> GetReportsOrdersMapping();
    }
}
