using PhoneBook.Services.Report.DTOs;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report.Services
{
    public interface IReportService
    {
        Task<ProcessResult<List<ReportDto>>> GetAllReportsAsync();
        Task<ProcessResult<ReportDto>> CreateReportAsync();
    }
}
