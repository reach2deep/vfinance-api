using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vfinance_api.Dto.Request;
using vfinance_api.Models;

namespace vfinance_api.Infrastructure.Configs
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<Expense, CreateExpenseRequest>().ReverseMap();
        }
    }
}
