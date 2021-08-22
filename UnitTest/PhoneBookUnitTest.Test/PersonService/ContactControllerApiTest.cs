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
    public class ContactControllerApiTest
    {
        private readonly Mock<IContactService> _mockContactService;
        private readonly ContactController _ContactController;

        private ContactCreateDto contactCreateDto;

        public ContactControllerApiTest()
        {
            _mockContactService = new Mock<IContactService>();
            _ContactController = new ContactController(_mockContactService.Object);

            contactCreateDto = new ContactCreateDto()
            {
                PersonUUID = "61218d200e49de54d687d89f",
                ContactTypeUUID = "61218d200e49de54d687d89a",
                Content = "Adres bilgileri"
            };
        }

        [Fact]
        public async void CreateContactToPersonAsync_SuccessReturnNoContent()
        {

            var resultSet = ProcessResult<NoContent>.Success(200);

            _mockContactService.Setup(x => x.CreateContactToPersonAsync(contactCreateDto)).ReturnsAsync(resultSet);

            var result = await _ContactController.CreateContactToPerson(contactCreateDto);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultContact = Assert.IsAssignableFrom<ProcessResult<NoContent>>(successResult.Value);

            _mockContactService.Verify(x => x.CreateContactToPersonAsync(contactCreateDto), Times.Once);


        }


        [Theory]
        [InlineData("61218d200e49de54d687d89f")]
        public async void DeleteContactFromAsync_SuccessReturnNoContent(string contactUUID)
        {

            var resultSet = ProcessResult<NoContent>.Success(204);

            _mockContactService.Setup(x => x.DeleteContactFromAsync(contactUUID)).ReturnsAsync(resultSet);

            var result = await _ContactController.DeleteContactFromAsync(contactUUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultContact = Assert.IsAssignableFrom<ProcessResult<NoContent>>(successResult.Value);

        }

        [Theory]
        [InlineData("61218d200e49de54d687d89f")]
        public async void DeleteContactFromAsync_ErrorReturnMessage(string contactUUID)
        {

            var resultSet = ProcessResult<NoContent>.Error("Person not found", 404);

            _mockContactService.Setup(x => x.DeleteContactFromAsync(contactUUID)).ReturnsAsync(resultSet);

            var result = await _ContactController.DeleteContactFromAsync(contactUUID);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultContact = Assert.IsAssignableFrom<ProcessResult<NoContent>>(successResult.Value);

        }
    }
}
