using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Services.Report.DTOs;
using PhoneBook.Services.Report.Services;
using PhoneBook.Shared.ControllerBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : CustomBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var response = await _reportService.GetAllReportsAsync();
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport()
        {
            var response = await _reportService.CreateReportAsync();
            return CreateActionResultInstance(response);
        }
    }
}
