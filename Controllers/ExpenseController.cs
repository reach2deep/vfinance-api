using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using vfinance_api.Contracts;
using vfinance_api.Dto.Request;
using vfinance_api.Helper;
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpenseResponse>), Status200OK)]
        public async Task<IEnumerable<ExpenseResponse>> Get()
        {
            var data = await _expenseManger.GetAllAsync();
            var expenses = _mapper.Map<IEnumerable<ExpenseResponse>>(data);

            return expenses;
        }

        [Route("paged")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpenseResponse>), Status200OK)]
        public async Task<IEnumerable<ExpenseResponse>> Get([FromQuery] UrlQueryParameters urlQueryParameters)
        {
            //var data = await _expenseManger.GetExpensesAsync(urlQueryParameters);
            //var expenses = _mapper.Map<IEnumerable<ExpenseResponse>>(data.Expenses);

            //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(data.Pagination));

            //return expenses;
            var data = await _expenseManger.GetAllAsync();
            var expenses = _mapper.Map<IEnumerable<ExpenseResponse>>(data);

            return expenses;
        }

        [Route("{id:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(ExpenseResponse), Status200OK)]
        [ProducesResponseType(typeof(ExpenseResponse), Status404NotFound)]
        public async Task<ExpenseResponse> Get(long id)
        {
            var person = await _expenseManger.GetByIdAsync(id);
            return person != null ? _mapper.Map<ExpenseResponse>(person)
                                  : throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", Status404NotFound);
        }

        [Route("{id:int}")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Put(int id, [FromBody] ExpenseResponse updateRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var expense = _mapper.Map<Expense>(updateRequest);
            expense.Id = id;

            if (await _expenseManger.UpdateAsync(expense))
            {
                return new ApiResponse($"Record with Id: {id} sucessfully updated.", true);
            }
            else
            {
                throw new ApiProblemDetailsException($"Record with Id: {id} does not exist.", Status404NotFound);
            }
        }

        [Route("{id:long}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        public async Task<ApiResponse> Delete(long id)
        {
            if (await _expenseManger.DeleteAsync(id))
            {
                return new ApiResponse($"Record with Id: {id} sucessfully deleted.", true);
            }
            else
            {
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", Status404NotFound);
            }
        }



    }
}
