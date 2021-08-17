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
    public class PersonService : IPersonService
    {
        private readonly IMongoCollection<Models.Person> _personCollection;
        private readonly IMongoCollection<Contact> _contactCollection;

        private readonly IMapper _mapper;

        public PersonService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _personCollection = database.GetCollection<Models.Person>(databaseSettings.PersonCollectionName);
            _contactCollection = database.GetCollection<Contact>(databaseSettings.ContactCollectionName);

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
                if (person.ContactList != null && person.ContactList.Count > 0)
                {
                    var resultContact = await _contactCollection.DeleteOneAsync(contact => person.ContactList.Contains(contact.UUID));
                }
                return ProcessResult<NoContent>.Success(204);
            }

            return ProcessResult<NoContent>.Error("Person not found", 404);
        }


        public async Task<ProcessResult<PersonDetailDto>> GetPersonWithDetailByPersonUUIDAsync(string personUUID)
        {
            //to do
            //var person = await _personCollection.Aggregate().Match(person => person.UUID == personUUID)
            //                                    .Lookup("Contact", "ContactList", "_id", "asContacts")
            //                                    .Lookup("ContactType", "ContactTypeUUID", "_id", "asContactType")
            //                                    .FirstAsync();

            //if (person == null)
            //{
            //    return ProcessResult<PersonDetailDto>.Error("There is no person", 404);
            //}

            //return ProcessResult<PersonDetailDto>.Success(_mapper.Map<PersonDetailDto>(person), 200);

            return ProcessResult<PersonDetailDto>.Success(new PersonDetailDto(), 200);
        }
    }
}