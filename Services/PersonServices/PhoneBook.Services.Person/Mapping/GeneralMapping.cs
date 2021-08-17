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
            CreateMap<ContactCreateDto, Contact>().ReverseMap();
            CreateMap<ContactDetailDto, Contact>().ReverseMap();
            CreateMap<PersonCreateDto, Models.Person>().ReverseMap();
            CreateMap<PersonDto, Models.Person>().ReverseMap();
            CreateMap<PersonDetailDto, Models.Person>().ReverseMap();
        }
    }
}
