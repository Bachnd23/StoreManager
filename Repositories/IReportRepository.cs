using COCOApp.Models;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IReportRepository
    {
        List<Report> GetReports(int customerId, int sellerId);
        void AddReport(Report report);
    }
}