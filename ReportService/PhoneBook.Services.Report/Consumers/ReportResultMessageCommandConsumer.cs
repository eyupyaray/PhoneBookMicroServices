using AutoMapper;
using MassTransit;
using MongoDB.Driver;
using PhoneBook.Services.Report.Settings;
using PhoneBook.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report.Consumers
{
    public class ReportResultMessageCommandConsumer : IConsumer<ReportResultMessageCommand>
    {
        private readonly IMongoCollection<Models.Report> _reportCollection;

        private readonly IMapper _mapper;
        public ReportResultMessageCommandConsumer(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _reportCollection = database.GetCollection<Models.Report>(databaseSettings.ReportCollectionName);

            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<ReportResultMessageCommand> context)
        {
            var report = _reportCollection.FindAsync(report => report.UUID == context.Message.ReportUUID).Result.FirstOrDefault();

            if (report != null)
            {
                if (report.ReportContent == null)
                    report.ReportContent = new List<Models.ReportContent>();

                context.Message.ReportContent.ForEach(x =>
                {
                    report.ReportContent.Add(new Models.ReportContent()
                    {

                        Location = x.Location,
                        PersonCount = x.PersonCount,
                        PhoneNumberCount = x.PhoneNumberCount
                    });
                });
            }

            report.ReportStatus = "Tamamlandı";

            var reportDto = _mapper.Map<Models.Report>(report);

            await _reportCollection.FindOneAndReplaceAsync(report => report.UUID == context.Message.ReportUUID, reportDto);
        }
    }
}
