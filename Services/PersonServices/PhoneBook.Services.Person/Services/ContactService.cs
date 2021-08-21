using AutoMapper;
using MongoDB.Bson;
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
    public class ContactService : IContactService
    {
        private readonly IMongoCollection<Models.Person> _personCollection;
        private readonly IMongoCollection<Contact> _contactCollection;

        private readonly IMapper _mapper;

        public ContactService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _personCollection = database.GetCollection<Models.Person>(databaseSettings.PersonCollectionName);
            _contactCollection = database.GetCollection<Contact>(databaseSettings.ContactCollectionName);

            _mapper = mapper;
        }


        public async Task<ProcessResult<NoContent>> CreateContactToPersonAsync(ContactCreateDto contact)
        {
            var contactDto = _mapper.Map<Contact>(contact);

            await _contactCollection.InsertOneAsync(contactDto);

            var person = _personCollection.FindAsync(person => person.UUID == contact.PersonUUID).Result.FirstOrDefault();

            var personDto = _mapper.Map<Models.Person>(person);

            await _personCollection.FindOneAndReplaceAsync(person => person.UUID == personDto.UUID, personDto);


            return ProcessResult<NoContent>.Success(200);
        }

        public async Task<ProcessResult<NoContent>> DeleteContactFromAsync(string contactUUID)
        {
            var result = await _contactCollection.DeleteOneAsync(contact => contact.UUID == contactUUID);

            if (result.DeletedCount > 0)
            {
                return ProcessResult<NoContent>.Success(204);
            }

            return ProcessResult<NoContent>.Error("Person not found", 404);
        }

    }
}

