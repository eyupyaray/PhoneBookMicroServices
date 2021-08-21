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

namespace PhoneBook.Services.Person.Services
{
    public class PersonService : IPersonService
    {
        private readonly IMongoCollection<Models.Person> _personCollection;
        private readonly IMongoCollection<Contact> _contactCollection;
        private readonly IMongoCollection<ContactType> _contactTypeCollection;

        private readonly IMapper _mapper;

        public PersonService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _personCollection = database.GetCollection<Models.Person>(databaseSettings.PersonCollectionName);
            _contactCollection = database.GetCollection<Contact>(databaseSettings.ContactCollectionName);
            _contactTypeCollection = database.GetCollection<ContactType>(databaseSettings.ContactTypeCollectionName);

            _mapper = mapper;
        }

        public async Task<ProcessResult<List<PersonDto>>> GetAllPeopleAsync()
        {
            var persons = await _personCollection.Find(type => true).ToListAsync();

            if (persons == null || persons.Count == 0)
            {
                return ProcessResult<List<PersonDto>>.Error("There is no person", 404);
            }

            return ProcessResult<List<PersonDto>>.Success(_mapper.Map<List<PersonDto>>(persons), 200);
        }

        public async Task<ProcessResult<PersonCreateDto>> CreatePersonAsync(PersonCreateDto person)
        {
            var personDto = _mapper.Map<Models.Person>(person);

            await _personCollection.InsertOneAsync(personDto);

            return ProcessResult<PersonCreateDto>.Success(_mapper.Map<PersonCreateDto>(personDto), 200);
        }

        public async Task<ProcessResult<NoContent>> DeletePersonAsync(string personUUID)
        {

            var person = _personCollection.FindAsync(person => person.UUID == personUUID).Result.FirstOrDefault();

            var result = await _personCollection.DeleteOneAsync(person => person.UUID == personUUID);

            if (result.DeletedCount > 0)
            {

                var resultContact = await _contactCollection.DeleteManyAsync(contact => contact.PersonUUID == personUUID);

                return ProcessResult<NoContent>.Success(204);
            }

            return ProcessResult<NoContent>.Error("Person not found", 404);
        }


        public async Task<ProcessResult<PersonDetailDto>> GetPersonWithDetailByPersonUUIDAsync(string personUUID)
        {

            PersonDetailDto personDetail = new PersonDetailDto();

            var personInformation = _personCollection.FindAsync(person => person.UUID == personUUID).Result.FirstOrDefault();

            if (personInformation == null)
                return ProcessResult<PersonDetailDto>.Error("Person not found", 404);

            personDetail = _mapper.Map<PersonDetailDto>(personInformation);

            var contactList = (from person in _personCollection.AsQueryable()
                               join contact in _contactCollection.AsQueryable() on person.UUID equals contact.PersonUUID
                               join contactType in _contactTypeCollection.AsQueryable() on contact.ContactTypeUUID equals contactType.UUID
                               where person.UUID == personUUID
                               select new ContactDetailDto
                               {
                                   UUID = contact.UUID,
                                   ContactType = contactType.Name,
                                   Content = contact.Content
                               }).ToList();

            personDetail.ContactList = contactList;


            return ProcessResult<PersonDetailDto>.Success(_mapper.Map<PersonDetailDto>(personDetail), 200);
        }


    }
}