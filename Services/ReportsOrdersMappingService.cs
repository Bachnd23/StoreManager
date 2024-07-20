using COCOApp.Models;
using COCOApp.Repositories;
using System.Collections.Generic;

namespace COCOApp.Services
{
    public class ReportsOrdersMappingService : StoreManagerService
    {
        private readonly IReportsOrdersMappingRepository _reportsOrdersMappingRepository;

        public ReportsOrdersMappingService(IReportsOrdersMappingRepository reportsOrdersMappingRepository)
        {
            _reportsOrdersMappingRepository = reportsOrdersMappingRepository;
        }

        public List<ReportsOrdersMapping> GetReportsOrdersMapping()
        {
            return _reportsOrdersMappingRepository.GetReportsOrdersMapping();
        }
    }
}
