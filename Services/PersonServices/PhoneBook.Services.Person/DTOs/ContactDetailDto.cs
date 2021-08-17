using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.DTOs
{
    public class ContactDetailDto
    {
        public string UUID { get; set; }
        public string ContactType { get; set; }
        public string Content { get; set; }
    }
}
