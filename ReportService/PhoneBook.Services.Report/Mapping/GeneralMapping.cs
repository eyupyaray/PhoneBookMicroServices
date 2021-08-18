﻿using AutoMapper;
using PhoneBook.Services.Report.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<ReportDto, Models.Report>().ReverseMap();
        }
    }
}
