using PhoneBook.Services.Person.DTOs;
using PhoneBook.Services.Person.Models;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.Services
{
    public interface IPersonService
    {
        Task<ProcessResult<List<PersonDto>>> GetAllPeopleAsync();
        Task<ProcessResult<PersonCreateDto>> CreatePersonAsync(PersonCreateDto personCreate);
        Task<ProcessResult<NoContent>> DeletePersonAsync(string personUUID);
        Task<ProcessResult<PersonDetailDto>> GetPersonWithDetailByPersonUUIDAsync(string personUUID);
    }
}
