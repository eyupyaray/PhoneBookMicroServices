using AutoMapper;
using MongoDB.Driver;
using PhoneBook.Services.Report.DTOs;
using PhoneBook.Services.Report.Settings;
using PhoneBook.Shared.Messages;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report.Services
{
    public class ReportService :IReportService
    {
        private readonly IMongoCollection<Models.Report> _reportCollection;

        private readonly IMapper _mapper;

        public ReportService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _reportCollection = database.GetCollection<Models.Report>(databaseSettings.ReportCollectionName);

            _mapper = mapper;
        }

        public async Task<ProcessResult<List<ReportDto>>> GetAllReportsAsync()
        {
            var reports = await _reportCollection.Find(type => true).ToListAsync();

            if (reports == null || reports.Count == 0)
            {
                return ProcessResult<List<ReportDto>>.Error("There is no contact type", 404);
            }

            return ProcessResult<List<ReportDto>>.Success(_mapper.Map<List<ReportDto>>(reports), 200);
        }

        public async Task<ProcessResult<ReportDto>> CreateReportAsync()
        {
            var report = new Models.Report();
            report.ReportStatus = "Hazırlanıyor";
            report.ReportRequestDate = DateTime.Now;

            await _reportCollection.InsertOneAsync(report);

            // to do rabbit mq message queue

            return ProcessResult<ReportDto>.Success(_mapper.Map<ReportDto>(report), 200);
        }

        public async Task<ProcessResult<ReportDetailDto>> GetReportWithDetailByReportUUIDAsync(string reportUUID)
        {

            var report =  _reportCollection.FindAsync(report => report.UUID == reportUUID).Result.FirstOrDefault();

            if (report == null)
            {
                return ProcessResult<ReportDetailDto>.Error("There is no contact type", 404);
            }

            return ProcessResult<ReportDetailDto>.Success(_mapper.Map<ReportDetailDto>(report), 200);
        }
    }
}
