using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Data;
using TestTask.Utils.DTOs;

namespace TestTask.Services
{
    public class ReportService
    {
        private readonly TestDbContext _testDbContext;

        public ReportService(TestDbContext testDbContext)
        {
            _testDbContext = testDbContext;
        }

        public async Task<List<YearlyReportDTO>> GetPopularityReport()
        {
            var popularityReport = await _testDbContext.UserItems
                .GroupBy(ui => new { ui.ItemId, ui.Item.Name, ui.PurchaseDate.Date, ui.UserId }) // group same item bought by same user on the same day
                .Select(g => new { g.Key.ItemId, g.Key.Name, g.Key.Date.Year, g.Key.UserId, TotalBuyOrders = g.Count() }) // create a list of entities from prev grouping
                .GroupBy(g => new { g.ItemId, g.Name, g.Year }) // group by item and year (as we need to get most popular in a year)
                .Select(g => new { g.Key.ItemId, g.Key.Name, g.Key.Year, TotalBuyOrders = g.Max(x => x.TotalBuyOrders) }) // create corresponding entities with the maximum total buy orders
                .OrderByDescending(x => x.TotalBuyOrders)
                .Select(x => new YearlyReportDTO
                {
                    ItemId = x.ItemId,
                    ItemName = x.Name,
                    TotalBuyOrders = x.TotalBuyOrders,
                    Year = x.Year
                })
                .ToListAsync();

            return popularityReport.GroupBy(x => x.Year).SelectMany(g => g.OrderByDescending(x => x.TotalBuyOrders).Take(3)).ToList();
        }
    }
}
