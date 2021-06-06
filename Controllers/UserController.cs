using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vfinance_api.DataManager;
using vfinance_api.Models;

namespace vfinance_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private vFinDbContext DbContext;

        public UserController(vFinDbContext context)
        {
            DbContext = context;
        }

        [HttpGet]
        public IList<AppUser> Get()
        {
            return (this.DbContext.AppUsers.ToList());
        }

        //[HttpPost]
        //[ProducesResponseType(typeof(ApiResponse), Status201Created)]
        //[ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        //public async Task<ApiResponse> Post([FromBody] CreatePersonRequest createRequest)
        //{
        //    if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

        //    var person = _mapper.Map<Person>(createRequest);
        //    return new ApiResponse("Record successfully created.", await _personManager.CreateAsync(person), Status201Created);
        //}
    }
}
