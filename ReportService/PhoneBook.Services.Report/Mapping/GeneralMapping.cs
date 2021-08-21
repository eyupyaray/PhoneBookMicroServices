using AutoMapper;
using PhoneBook.Services.Report.DTOs;
using PhoneBook.Services.Report.Models;
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
            CreateMap<ReportContentDto, ReportContent>().ReverseMap();
            CreateMap<ReportDetailDto, Models.Report>().ReverseMap();
        }
    }
}
