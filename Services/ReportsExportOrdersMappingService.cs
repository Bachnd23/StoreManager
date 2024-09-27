using COCOApp.Models;
using COCOApp.Repositories;
using System.Collections.Generic;

namespace COCOApp.Services
{
    public class ReportsExportOrdersMappingService : StoreManagerService
    {
        private readonly IReportsExportOrdersMappingRepository _reportsOrdersMappingRepository;

        public ReportsExportOrdersMappingService(IReportsExportOrdersMappingRepository reportsOrdersMappingRepository)
        {
            _reportsOrdersMappingRepository = reportsOrdersMappingRepository;
        }

        public List<ReportsExportOrdersMapping> GetReportsExportOrdersMapping()
        {
            return _reportsOrdersMappingRepository.GetReportsOrdersMapping();
        }
    }
}
