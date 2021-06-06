using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vfinance_api.Contracts;
using vfinance_api.Helper;
using vfinance_api.Models;

namespace vfinance_api.DataManager
{
    public class ExpenseManger : DbFactoryBase, IExpenseManager
    {
        private readonly ILogger<ExpenseManger> _logger;

        public ExpenseManger(IConfiguration config, ILogger<ExpenseManger> logger) : base(config, logger)
        {
            _logger = logger;
        }

        public async Task<long> CreateAsync(Expense entity)
        {
            //string sqlQuery = $@"INSERT INTO Expenses (FirstName, LastName, DateOfBirth) 
            //                         VALUES (@FirstName, @LastName, @DateOfBirth)
            //                         SELECT CAST(SCOPE_IDENTITY() as bigint)";

            entity.IsActive = true;
            entity.CreationAt = DateTime.Now;
            entity.CreatedBy = "Admin";

            string sqlQuery = $@"INSERT INTO Expenses (ExpenseDate, Category, Amount, Description, CreationAt, 
                                        CreatedBy, ModifiedAt, ModifiedBy, IsActive) 
                                        VALUES (@ExpenseDate,@Category,@Amount,@Description,@CreationAt,@CreatedBy,@ModifiedAt,@ModifiedBy,@IsActive)";

            return await DbQuerySingleAsync<long>(sqlQuery, entity);
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Expense>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Expense> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<Expense> Persons, Pagination Pagination)> GetPersonsAsync(UrlQueryParameters urlQueryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Expense entity)
        {
            throw new NotImplementedException();
        }
    }
}
