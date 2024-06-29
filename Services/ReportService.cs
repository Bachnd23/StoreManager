using COCOApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace COCOApp.Services
{
    public class ReportService : StoreManagerService
    {
        public List<Report> GetReports(int customerId,int sellerId)
        {

            try
            {
                var query=_context.Reports.AsQueryable();
                if(sellerId > 0)
                {
                    query=query.Where(r=>r.SellerId == sellerId);   
                }
                 query = query.Include(r => r.Customer)
                              .Include(r => r.ReportsOrdersMappings)
                              .Where(r => r.CustomerId == customerId);

                return query.ToList();
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Debug.WriteLine($"Error retrieving reports: {ex.Message}");
                throw new ApplicationException("Error retrieving reports", ex);
            }
        }
        public void AddReport(Report report)
        {
            try
            {
                _context.Reports.Add(report);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Debug.WriteLine($"Error retrieving reports: {ex.Message}");
                throw new ApplicationException("Error retrieving reports", ex);
            }
        }
    }
}
