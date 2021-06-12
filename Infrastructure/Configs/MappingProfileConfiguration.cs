using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vfinance_api.Dto;
using vfinance_api.Dto.Request;
using vfinance_api.Models;

namespace vfinance_api.Infrastructure.Configs
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<Expense, CreateExpenseRequest>().ReverseMap();
            CreateMap<Expense, ExpenseResponse>().ReverseMap();

            CreateMap<Attachment, AttachmentRequest>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Loan, LoanDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();


            //CreateMap<Expense, ExpenseResponse>();
            //CreateMap<ExpenseResponse, Expense>();
        }
    }
}
