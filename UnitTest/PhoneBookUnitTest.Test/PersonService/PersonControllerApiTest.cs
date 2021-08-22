using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneBook.Services.Person.Controllers;
using PhoneBook.Services.Person.DTOs;
using PhoneBook.Services.Person.Services;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhoneBookUnitTest.Test.PersonService
{
    public class PersonControllerApiTest
    {
        private readonly Mock<IPersonService> _mockPersonService;
        private readonly PersonController _personController;

        private List<PersonDto> persons;
        private PersonDetailDto personDetail;
        private PersonCreateDto createdPerson;

        public PersonControllerApiTest()
        {
            _mockPersonService = new Mock<IPersonService>();
            _personController = new PersonController(_mockPersonService.Object);

            persons = new List<PersonDto>()
            {
                new PersonDto
                {
                  UUID = "61218d3d0e49de54d687d8a0",
                  Name = "Eyüp",
                  Surname = "Yaray",
                  CompanyName = "Eyup's Company"
                },
                new PersonDto
                {
                  UUID = "61218d3d0e49de54d687d8aa",
                  Name = "Ahmet",
                  Surname = "Deneme",
                  CompanyName = "Ahmet's Company"
                },
                new PersonDto
                {
                  UUID = "61218d3d0e49de54d687d8ae",
                  Name = "Mehmet",
                  Surname = "Deneme",
                  CompanyName = "Mehmet's Company"
                }
            };

            personDetail = new PersonDetailDto
            {
                UUID = "61218d3d0e49de54d687d8a0",
                Name = "Eyüp",
                Surname = "Yaray",
                CompanyName = "Eyup's Company",
                ContactList = new List<ContactDetailDto>()
                {
                    new ContactDetailDto
                    {
                        UUID= "61218dc00e49de54d687d8a6",
                        ContactType= "Konum",
                        Content= "trabzon"
                    }
                }
            };

            createdPerson = new PersonCreateDto()
            {
                Name = "Ali",
                Surname = "Deneme",
                CompanyName = "Ali's Company"
            };
        }

        [Fact]
        public async void GetAllPeople_SuccessReturnPersonList()
        {
            var resultSet = ProcessResult<List<PersonDto>>.Success(persons, 200);

            _mockPersonService.Setup(x => x.GetAllPeopleAsync()).ReturnsAsync(resultSet);

            var result = await _personController.GetAllPeople();

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultPerson = Assert.IsAssignableFrom<ProcessResult<List<PersonDto>>>(successResult.Value);

            Assert.Equal<int>(3, resultPerson.Data.Count);

        }

        [Fact]
        public async void GetAllPeople_NotFoundReturnPersonList()
        {
            var resultSet = ProcessResult<List<PersonDto>>.Error("There is no person", 404);

            _mockPersonService.Setup(x => x.GetAllPeopleAsync()).ReturnsAsync(resultSet);

            var result = await _personController.GetAllPeople();

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultPerson = Assert.IsAssignableFrom<ProcessResult<List<PersonDto>>>(successResult.Value);

            Assert.Equal("There is no person", resultPerson.Messages[0]);

        }

        [Fact]
        public async void GetPersonWithDetailByPersonUUID_SuccessReturnPerson()
        {
            var person = persons.First();

            var resultSet = ProcessResult<PersonDetailDto>.Success(personDetail, 200);

            _mockPersonService.Setup(x => x.GetPersonWithDetailByPersonUUIDAsync(person.UUID)).ReturnsAsync(resultSet);

            var result = await _personController.GetPersonWithDetailByPersonUUID(person.UUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultPerson = Assert.IsAssignableFrom<ProcessResult<PersonDetailDto>>(successResult.Value);

            Assert.Equal(person.UUID, resultPerson.Data.UUID);
            Assert.Equal(person.Name, resultPerson.Data.Name);
            Assert.NotNull(resultPerson.Data.ContactList);

        }

        [Theory]
        [InlineData("61219bf4877124f920eece41")]
        public async void GetPersonWithDetailByPersonUUID_NotFoundReturnPerson(string personUUID)
        {
            var resultSet = ProcessResult<PersonDetailDto>.Error("There is no person", 200);

            _mockPersonService.Setup(x => x.GetPersonWithDetailByPersonUUIDAsync(personUUID)).ReturnsAsync(resultSet);

            var result = await _personController.GetPersonWithDetailByPersonUUID(personUUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultPerson = Assert.IsAssignableFrom<ProcessResult<PersonDetailDto>>(successResult.Value);

            Assert.Equal("There is no person", resultPerson.Messages[0]);

        }

        [Fact]
        public async void CreatePersonAsync_SuccessReturnPerson()
        {

            var resultSet = ProcessResult<PersonCreateDto>.Success(createdPerson, 200);

            _mockPersonService.Setup(x => x.CreatePersonAsync(createdPerson)).ReturnsAsync(resultSet);

            var result = await _personController.CreatePerson(createdPerson);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultPerson = Assert.IsAssignableFrom<ProcessResult<PersonCreateDto>>(successResult.Value);

            _mockPersonService.Verify(x => x.CreatePersonAsync(createdPerson), Times.Once);

            Assert.Equal(createdPerson.Name, resultPerson.Data.Name);

        }

        [Theory]
        [InlineData("61218d200e49de54d687d89f")]
        public async void DeletePersonAsync_SuccessReturnNoContent(string personUUID)
        {

            var resultSet = ProcessResult<NoContent>.Success(204);

            _mockPersonService.Setup(x => x.DeletePersonAsync(personUUID)).ReturnsAsync(resultSet);

            var result = await _personController.DeletePerson(personUUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultPerson = Assert.IsAssignableFrom<ProcessResult<NoContent>>(successResult.Value);

        }

        [Theory]
        [InlineData("61218d200e49de54d687d89f")]
        public async void DeletePersonAsync_ErrorReturnMessage(string personUUID)
        {

            var resultSet = ProcessResult<NoContent>.Error("Person not found", 404);

            _mockPersonService.Setup(x => x.DeletePersonAsync(personUUID)).ReturnsAsync(resultSet);

            var result = await _personController.DeletePerson(personUUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultPerson = Assert.IsAssignableFrom<ProcessResult<NoContent>>(successResult.Value);

        }

    }
}
