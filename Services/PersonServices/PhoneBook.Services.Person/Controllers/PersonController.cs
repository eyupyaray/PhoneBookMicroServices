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
    public class PersonController : CustomBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPeople()
        {
            var response = await _personService.GetAllPeopleAsync();
            return CreateActionResultInstance(response);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPersonWithDetailByPersonUUID(string personUUID)
        {
            var response = await _personService.GetPersonWithDetailByPersonUUIDAsync(personUUID);
            return CreateActionResultInstance(response);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePerson(PersonCreateDto person)
        {
            var response = await _personService.CreatePersonAsync(person);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeletePerson(string personUUID)
        {
            var response = await _personService.DeletePersonAsync(personUUID);
            return CreateActionResultInstance(response);
        }
    }
}
