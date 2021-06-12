using Dapper;
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

        public async Task<bool> DeleteAsync(object id)
        {
            string sqlQuery = $@"UPDATE Expenses SET IsActive = false WHERE ID = @ID";

            return await DbExecuteAsync<bool>(sqlQuery, new { id });
        }

        public Task<bool> ExistAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            return await DbQueryAsync<Expense>("SELECT * FROM Expenses WHERE IsActive=true");
        }

        public async Task<Expense> GetByIdAsync(object id)
        {
            return await DbQuerySingleAsync<Expense>("SELECT * FROM Expenses WHERE Id = @ID", new { id });
        }

        public async Task<(IEnumerable<Expense> Expenses, Pagination Pagination)> GetExpensesAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<Expense> expenses;
            int recordCount = default;

            ////For PosgreSql
            var query = @"SELECT * FROM Expenses
                            ORDER BY ID DESC 
                            Limit @Limit Offset @Offset";


            ////For SqlServer
            //var query = @"SELECT * FROM Expenses
            //                ORDER BY ID DESC
            //                OFFSET @Limit * (@Offset -1) ROWS
            //                FETCH NEXT @Limit ROWS ONLY";

            var param = new DynamicParameters();
            param.Add("Limit", urlQueryParameters.PageSize);
            param.Add("Offset", urlQueryParameters.PageNumber);

            if (urlQueryParameters.IncludeCount)
            {
                query += " SELECT COUNT(ID) FROM Expenses";
                var pagedRows = await DbQueryMultipleAsync<Expense, int>(query, param);

                expenses = pagedRows.Data;
                recordCount = pagedRows.RecordCount;
            }
            else
            {
                expenses = await DbQueryAsync<Expense>(query, param);
            }

            var metadata = new Pagination
            {
                PageNumber = urlQueryParameters.PageNumber,
                PageSize = urlQueryParameters.PageSize,
                TotalRecords = recordCount

            };

            return (expenses, metadata);
        }

        public async Task<bool> UpdateAsync(Expense entity)
        {
            string sqlQuery = $@"UPDATE Expenses SET ExpenseDate = @ExpenseDate, 
                                            Category = @Category,
                                            Amount = @Amount,
                                            Description = @Description,
                                            CreationAt = @CreationAt,
                                            CreatedBy = @CreatedBy,
                                            ModifiedAt = @ModifiedAt,
                                            ModifiedBy = @ModifiedBy,
                                            IsActive = @IsActive
                                            WHERE Id = @Id";

            return await DbExecuteAsync<bool>(sqlQuery, entity);
        }
    }
}
