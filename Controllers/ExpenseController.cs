using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using vfinance_api.Contracts;
using vfinance_api.Dto.Request;
using vfinance_api.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace vfinance_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly IExpenseManager _expenseManger;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseManager expenseManger, IMapper mapper, ILogger<ExpenseController> logger)
        {
            _expenseManger = expenseManger;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Post([FromBody] CreateExpenseRequest createRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var person = _mapper.Map<Expense>(createRequest);
            return new ApiResponse("Record successfully created.", await _expenseManger.CreateAsync(person), Status201Created);
        }
    }
}
