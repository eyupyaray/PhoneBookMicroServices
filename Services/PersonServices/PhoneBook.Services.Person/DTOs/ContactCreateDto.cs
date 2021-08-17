using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.DTOs
{
    public class ContactCreateDto
    {
        public string PersonUUID { get; set; }
        public string ContactTypeUUID { get; set; }
        public string Content { get; set; }

    }
}
