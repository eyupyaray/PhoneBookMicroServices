using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report.DTOs
{
    public class ReportDetailDto
    {
        public string UUID { get; set; }
        public DateTime ReportRequestData { get; set; }
        public string ReportStatus { get; set; }
        public ReportContentDto ReportContent { get; set; }
    }
}
