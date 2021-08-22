using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneBook.Services.Report.Controllers;
using PhoneBook.Services.Report.DTOs;
using PhoneBook.Services.Report.Models;
using PhoneBook.Services.Report.Services;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PhoneBookUnitTest.Test.ReportService
{
    public class ReportControllerApiTest
    {
        private readonly Mock<IReportService> _mockReportService;
        private readonly Mock<ISendEndpointProvider> _mockISendEndpointProvider;
        private readonly ReportController _reportController;

        private List<ReportDto> reports;
        private ReportDetailDto reportDetail;

        public ReportControllerApiTest()
        {
            _mockReportService = new Mock<IReportService>();
            _mockISendEndpointProvider = new Mock<ISendEndpointProvider>();
            _reportController = new ReportController(_mockReportService.Object, _mockISendEndpointProvider.Object);

            reports = new List<ReportDto>()
            {
                new ReportDto
                {
                    UUID="61218df27639859cfa69ef62",
                    ReportRequestDate= DateTime.Now,
                    ReportStatus= "Tamamlandı"
                },
                new ReportDto
                {
                    UUID="61219bf2877124f920eece40",
                    ReportRequestDate= DateTime.Now,
                    ReportStatus= "Hazırlanıyor"
                },
                new ReportDto
                {
                    UUID="61219bf4877124f920eece41",
                    ReportRequestDate= DateTime.Now,
                    ReportStatus= "Tamamlandı"
                }
            };

            reportDetail = new ReportDetailDto
            {
                UUID = "61219bf4877124f920eece41",
                ReportRequestDate = DateTime.Now,
                ReportStatus = "Tamamlandı",
                ReportContent = new List<ReportContentDto>()
                {
                    new ReportContentDto
                    {
                        Location = "ankara",
                        PersonCount = 1,
                        PhoneNumberCount = 2
                    }
                }
            };

        }

        [Fact]
        public async void GetAllReports_SuccessReturnReportList()
        {
            var resultSet = ProcessResult<List<ReportDto>>.Success(reports, 200);

            _mockReportService.Setup(x => x.GetAllReportsAsync()).ReturnsAsync(resultSet);

            var result = await _reportController.GetAllReports();

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultReport = Assert.IsAssignableFrom<ProcessResult<List<ReportDto>>>(successResult.Value);

            Assert.Equal<int>(3, resultReport.Data.Count);

        }

        [Fact]
        public async void GetAllReports_NotFoundReturnReportList()
        {
            var resultSet = ProcessResult<List<ReportDto>>.Error("There is no report", 404);

            _mockReportService.Setup(x => x.GetAllReportsAsync()).ReturnsAsync(resultSet);

            var result = await _reportController.GetAllReports();

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultReport = Assert.IsAssignableFrom<ProcessResult<List<ReportDto>>>(successResult.Value);

            Assert.Equal("There is no report", resultReport.Messages[0]);

        }

        [Theory]
        [InlineData("61219bf4877124f920eece41")]
        public async void GetReportWithDetailByReportUUID_SuccessReturnReport(string reportUUID)
        {
            var resultSet = ProcessResult<ReportDetailDto>.Success(reportDetail, 200);

            _mockReportService.Setup(x => x.GetReportWithDetailByReportUUIDAsync(reportUUID)).ReturnsAsync(resultSet);

            var result = await _reportController.GetReportWithDetailByReportUUID(reportUUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultReport = Assert.IsAssignableFrom<ProcessResult<ReportDetailDto>>(successResult.Value);

            Assert.Equal("61219bf4877124f920eece41", resultReport.Data.UUID);
            Assert.Equal("Tamamlandı", resultReport.Data.ReportStatus);
            Assert.NotNull(resultReport.Data.ReportContent);

        }

        [Theory]
        [InlineData("61219bf4877124f920eece41")]
        public async void GetReportWithDetailByReportUUID_NotFoundReturnReport(string reportUUID)
        {
            var resultSet = ProcessResult<ReportDetailDto>.Error("There is no report", 200);

            _mockReportService.Setup(x => x.GetReportWithDetailByReportUUIDAsync(reportUUID)).ReturnsAsync(resultSet);

            var result = await _reportController.GetReportWithDetailByReportUUID(reportUUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultReport = Assert.IsAssignableFrom<ProcessResult<ReportDetailDto>>(successResult.Value);

            Assert.Equal("There is no report", resultReport.Messages[0]);

        }


    }
}
