using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Services.Person.DTOs;
using PhoneBook.Services.Person.Models;
using PhoneBook.Services.Person.Services;
using PhoneBook.Shared.ControllerBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : CustomBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactToPerson(ContactCreateDto contact)
        {
            var response = await _contactService.CreateContactToPersonAsync(contact);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteContactFromAsync(string contactUUID)
        {
            var response = await _contactService.DeleteContactFromAsync(contactUUID);
            return CreateActionResultInstance(response);
        }
    }
}
