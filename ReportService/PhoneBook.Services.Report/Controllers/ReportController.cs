using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Services.Report.DTOs;
using PhoneBook.Services.Report.Services;
using PhoneBook.Shared.ControllerBases;
using PhoneBook.Shared.Messages;
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
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public ReportController(IReportService reportService, ISendEndpointProvider sendEndpointProvider)
        {
            _reportService = reportService;
            _sendEndpointProvider = sendEndpointProvider;
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

            //rabbitmq
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:report-message-service"));

            var reportDetailMessageCommand = new ReportDetailMessageCommand();

            reportDetailMessageCommand.ReportUUID = response.Data.UUID;

            await sendEndpoint.Send<ReportDetailMessageCommand>(reportDetailMessageCommand);

            return CreateActionResultInstance(response);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetReportWithDetailByReportUUID(string reportUUID)
        {
            var response = await _reportService.GetReportWithDetailByReportUUIDAsync(reportUUID);

            return CreateActionResultInstance(response);
        }
    }
}
