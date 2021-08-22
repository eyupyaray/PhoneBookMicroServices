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
    public class ContactTypeControllerApiTest
    {
        private readonly Mock<IContactTypeService> _mockContactTypeService;
        private readonly ContactTypeController _contactTypeController;

        private List<ContactTypeDto> contactTypes;

        public ContactTypeControllerApiTest()
        {
            _mockContactTypeService = new Mock<IContactTypeService>();
            _contactTypeController = new ContactTypeController(_mockContactTypeService.Object);

            contactTypes = new List<ContactTypeDto>()
            {
                new ContactTypeDto
                {
                    Name="Telefon Numarası",
                    UUID="61218d020e49de54d687d89d"
                },
                new ContactTypeDto
                {
                    Name="E-mail Adresi",
                    UUID="61218d140e49de54d687d89e"
                },
                new ContactTypeDto
                {
                    Name="Konum",
                    UUID="61218d200e49de54d687d89f"
                }
            };

        }

        [Fact]
        public async void GetAllContactTypesAsync_SuccessReturnContactTypeList()
        {
            var resultSet = ProcessResult<List<ContactTypeDto>>.Success(contactTypes, 200);

            _mockContactTypeService.Setup(x => x.GetAllContactTypesAsync()).ReturnsAsync(resultSet);

            var result = await _contactTypeController.GetAllContactTypes();

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultContactType = Assert.IsAssignableFrom<ProcessResult<List<ContactTypeDto>>>(successResult.Value);

            Assert.Equal<int>(3, resultContactType.Data.Count);

        }

        [Fact]
        public async void GetAllContactTypesAsync_NotFoundReturnContactTypeList()
        {
            var resultSet = ProcessResult<List<ContactTypeDto>>.Error("There is no contact type", 404);

            _mockContactTypeService.Setup(x => x.GetAllContactTypesAsync()).ReturnsAsync(resultSet);

            var result = await _contactTypeController.GetAllContactTypes();

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultContactType = Assert.IsAssignableFrom<ProcessResult<List<ContactTypeDto>>>(successResult.Value);

            Assert.Equal("There is no contact type", resultContactType.Messages[0]);

        }

        [Fact]
        public async void CreateContactTypeAsync_NotFoundReturnContactTypeList()
        {
            var contentType = contactTypes.First();

            var resultSet = ProcessResult<ContactTypeDto>.Success(contentType, 200);

            _mockContactTypeService.Setup(x => x.CreateContactTypeAsync(contentType)).ReturnsAsync(resultSet);

            var result = await _contactTypeController.CreateContactType(contentType);

            var successResult = Assert.IsType<ObjectResult>(result);

            var resultContactType = Assert.IsAssignableFrom<ProcessResult<ContactTypeDto>>(successResult.Value);

            _mockContactTypeService.Verify(x => x.CreateContactTypeAsync(contentType), Times.Once);

            Assert.Equal(contentType.Name, resultContactType.Data.Name);

        }

    }


}
