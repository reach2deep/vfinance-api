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
using vfinance_api.Dto;
using vfinance_api.Dto.Request;
using vfinance_api.Helper;
using vfinance_api.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace vfinance_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILogger<LoanController> _logger;
        private readonly ILoanManager _loanManger;
        private readonly IMapper _mapper;

        public LoanController(ILoanManager loanManger, IMapper mapper, ILogger<LoanController> logger)
        {
            _loanManger = loanManger;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Post([FromBody] LoanDto createRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var loan = _mapper.Map<Loan>(createRequest);
            return new ApiResponse("Record successfully created.", await _loanManger.CreateAsync(loan), Status201Created);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LoanDto>), Status200OK)]
        public async Task<IEnumerable<LoanDto>> Get()
        {
            var data = await _loanManger.GetAllAsync();
            var loans = _mapper.Map<IEnumerable<LoanDto>>(data);

            return loans;
        }

        [Route("paged")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LoanDto>), Status200OK)]
        public async Task<IEnumerable<LoanDto>> Get([FromQuery] UrlQueryParameters urlQueryParameters)
        {
            var data = await _loanManger.GetAllAsync();
            var loans = _mapper.Map<IEnumerable<LoanDto>>(data);

            return loans;
        }

        [Route("{id:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(LoanDto), Status200OK)]
        [ProducesResponseType(typeof(LoanDto), Status404NotFound)]
        public async Task<LoanDto> Get(long id)
        {
            var person = await _loanManger.GetByIdAsync(id);
            return person != null ? _mapper.Map<LoanDto>(person)
                                  : throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", Status404NotFound);
        }

        [Route("{id:int}")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Put(int id, [FromBody] LoanDto updateRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var loan = _mapper.Map<Loan>(updateRequest);
            loan.Id = id;

            if (await _loanManger.UpdateAsync(loan))
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
            if (await _loanManger.DeleteAsync(id))
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
