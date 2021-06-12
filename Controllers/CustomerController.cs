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
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerManager _customerManger;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerManager customerManger, IMapper mapper, ILogger<CustomerController> logger)
        {
            _customerManger = customerManger;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Post([FromBody] CustomerDto createRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var customer = _mapper.Map<Customer>(createRequest);
            return new ApiResponse("Record successfully created.", await _customerManger.CreateAsync(customer), Status201Created);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerDto>), Status200OK)]
        public async Task<IEnumerable<CustomerDto>> Get()
        {
            var data = await _customerManger.GetAllAsync();
            var customers = _mapper.Map<IEnumerable<CustomerDto>>(data);

            return customers;
        }

        [Route("paged")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerDto>), Status200OK)]
        public async Task<IEnumerable<CustomerDto>> Get([FromQuery] UrlQueryParameters urlQueryParameters)
        {
            //var data = await _customerManger.GetCustomersAsync(urlQueryParameters);
            //var customers = _mapper.Map<IEnumerable<CustomerDto>>(data.Customers);

            //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(data.Pagination));

            //return customers;
            var data = await _customerManger.GetAllAsync();
            var customers = _mapper.Map<IEnumerable<CustomerDto>>(data);

            return customers;
        }

        [Route("{id:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerDto), Status200OK)]
        [ProducesResponseType(typeof(CustomerDto), Status404NotFound)]
        public async Task<CustomerDto> Get(long id)
        {
            var person = await _customerManger.GetByIdAsync(id);
            return person != null ? _mapper.Map<CustomerDto>(person)
                                  : throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", Status404NotFound);
        }

        [Route("{id:int}")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Put(int id, [FromBody] CustomerDto updateRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var customer = _mapper.Map<Customer>(updateRequest);
            customer.Id = id;

            if (await _customerManger.UpdateAsync(customer))
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
            if (await _customerManger.DeleteAsync(id))
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
