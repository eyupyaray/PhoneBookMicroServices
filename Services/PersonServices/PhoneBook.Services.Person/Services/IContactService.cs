using PhoneBook.Services.Person.DTOs;
using PhoneBook.Services.Person.Models;
using PhoneBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.Services
{
    public interface IContactService
    {
        Task<ProcessResult<NoContent>> CreateContactToPersonAsync(ContactCreateDto contact);
        Task<ProcessResult<NoContent>> DeleteContactFromAsync(string contactUUID);
    }
}
