using AutoMapper;
using MongoDB.Driver;
using PhoneBook.Services.Person.DTOs;
using PhoneBook.Services.Person.Models;
using PhoneBook.Services.Person.Settings;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.Services
{
    public class ContactTypeService : IContactTypeService
    {
        private readonly IMongoCollection<ContactType> _contactTypeCollection;

        private readonly IMapper _mapper;

        public ContactTypeService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _contactTypeCollection = database.GetCollection<ContactType>(databaseSettings.ContactTypeCollectionName);

            _mapper = mapper;
        }

        public async Task<ProcessResult<List<ContactTypeDto>>> GetAllContactTypesAsync()
        {
            var contactTypes = await _contactTypeCollection.Find(type => true).ToListAsync();

            if(contactTypes == null || contactTypes.Count == 0)
            {
                return ProcessResult<List<ContactTypeDto>>.Error("There is no contact type", 404);
            }

            return ProcessResult<List<ContactTypeDto>>.Success(_mapper.Map<List<ContactTypeDto>>(contactTypes), 200);
        }

        public async Task<ProcessResult<ContactTypeDto>> CreateContactTypeAsync(ContactTypeDto contactType)
        {
            var contactTypeDto = _mapper.Map<ContactType>(contactType);

            await _contactTypeCollection.InsertOneAsync(contactTypeDto);

            return ProcessResult<ContactTypeDto>.Success(_mapper.Map<ContactTypeDto>(contactType), 200);
        }
    }
}

