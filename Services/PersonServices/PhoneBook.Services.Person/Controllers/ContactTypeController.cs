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
    public class ContactTypeController : CustomBase
    {
        private readonly IContactTypeService _contactTypeService;

        public ContactTypeController(IContactTypeService contactTypeService)
        {
            _contactTypeService = contactTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContactTypes()
        {
            var response = await _contactTypeService.GetAllContactTypesAsync();
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactType(ContactTypeDto contactType)
        {
            var response = await _contactTypeService.CreateContactTypeAsync(contactType);
            return CreateActionResultInstance(response);
        }
    }
}
