using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PhoneBook.Services.Person.DTOs;
using PhoneBook.Services.Person.Models;
using PhoneBook.Services.Person.Settings;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.Services.ReportServices
{
    public class ReportService 
    {
        private readonly IMongoCollection<Models.Person> _personCollection;
        private readonly IMongoCollection<Contact> _contactCollection;
        private readonly IMongoCollection<ContactType> _contactTypeCollection;

        private readonly IMapper _mapper;

        public ReportService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _personCollection = database.GetCollection<Models.Person>(databaseSettings.PersonCollectionName);
            _contactCollection = database.GetCollection<Contact>(databaseSettings.ContactCollectionName);
            _contactTypeCollection = database.GetCollection<ContactType>(databaseSettings.ContactTypeCollectionName);

            _mapper = mapper;
        }

        public async Task<ProcessResult<List<ReportContentDto>>> GetReportDetailByReportUUIDAsync(string reportUUID)
        {

            List<ReportContentDto> contentList = (from person in _personCollection.AsQueryable()
                               join contact in _contactCollection.AsQueryable() on person.UUID equals contact.PersonUUID
                               join contactType in _contactTypeCollection.AsQueryable() on contact.ContactTypeUUID equals contactType.UUID
                               where contactType.Name == "Konum"
                               select new
                               {
                                   UUID = contact.UUID,
                                   ContactType = contactType.Name,
                                   Content = contact.Content
                               }).ToList().AsEnumerable()
                                 .GroupBy(s => s.Content)
                                 .Select(n => new ReportContentDto
                                 {
                                     Location = n.Key,
                                     PersonCount = n.Count(),
                                     PhoneNumberCount = 0
                                 }).ToList();


            foreach (var reportContent in contentList)
            {
                reportContent.PhoneNumberCount = (from person in _personCollection.AsQueryable()
                                            join contact in _contactCollection.AsQueryable() on person.UUID equals contact.PersonUUID
                                            join contactLocation in _contactCollection.AsQueryable() on person.UUID equals contactLocation.PersonUUID
                                            join contactType in _contactTypeCollection.AsQueryable() on contact.ContactTypeUUID equals contactType.UUID
                                            join contactTypeLocation in _contactTypeCollection.AsQueryable() on contactLocation.ContactTypeUUID equals contactTypeLocation.UUID
                                            where contactType.Name == "Telefon Numarası"
                                            && contactTypeLocation.Name == "Konum" && contactLocation.Content == reportContent.Location
                                                  select new { UUID = contact.UUID  }
                                                         ).ToList().Count();
            }



            return ProcessResult<List<ReportContentDto>>.Success(_mapper.Map<List<ReportContentDto>>(contentList), 200);
        }
    }
}