using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Utils.DTOs
{
    public record YearlyReportDTO
    {
        public int ItemId { get; set; }

        public int Year { get; set; }

        public string ItemName { get; set; } = null!;

        public int TotalBuyOrders { get; set; }
    }
}
