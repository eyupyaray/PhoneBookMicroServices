using AutoMapper;
using PhoneBook.Services.Person.DTOs;
using PhoneBook.Services.Person.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<ContactTypeDto, ContactType>().ReverseMap();
        }
    }
}
