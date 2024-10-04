using COCOApp.Models;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IReportRepository
    {
        List<Report> GetReports(int customerId, int sellerId);
        List<ReportDetail> GetReportDetails(int orderId);
        void AddReport(Report report);
        void AddReportDetails(ReportDetail reportDetail);
    }
}