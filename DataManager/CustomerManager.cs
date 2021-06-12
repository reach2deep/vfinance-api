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
    public class CustomerManager : DbFactoryBase, ICustomerManager
    {
        private readonly ILogger<CustomerManager> _logger;

        public CustomerManager(IConfiguration config, ILogger<CustomerManager> logger) : base(config, logger)
        {
            _logger = logger;
        }

        public async Task<long> CreateAsync(Customer entity)
        {
            //string sqlQuery = $@"INSERT INTO Expenses (FirstName, LastName, DateOfBirth) 
            //                         VALUES (@FirstName, @LastName, @DateOfBirth)
            //                         SELECT CAST(SCOPE_IDENTITY() as bigint)";

            entity.IsActive = true;
            entity.CreationAt = DateTime.Now;
            entity.CreatedBy = "Admin";

            ////string sqlQuery = $@"INSERT INTO Customerrs (ExpenseDate, Category, Amount, Description, CreationAt, 
            ////                            CreatedBy, ModifiedAt, ModifiedBy, IsActive) 
            ////                            VALUES (@ExpenseDate,@Category,@Amount,@Description,@CreationAt,@CreatedBy,@ModifiedAt,@ModifiedBy,@IsActive)";

            string sqlQuery = $@"INSERT INTO Customers (Id,DisplayName,FirstName,LastName,Notes,Address,Mobile1,Mobile2,BusinessName,BusinessAddress,BusinessMobile1,BusinessMobile2,CreationAt,CreatedBy,ModifiedAt,ModifiedBy,IsActive)
                                    VALUES (@Id,@DisplayName,@FirstName,@LastName,@Notes,@Address,@Mobile1,@Mobile2,@BusinessName,@BusinessAddress,@BusinessMobile1,@BusinessMobile2,@CreationAt,@CreatedBy,@ModifiedAt,@ModifiedBy,@IsActive)";

            return await DbQuerySingleAsync<long>(sqlQuery, entity);
        }


        public async Task<bool> DeleteAsync(object id)
        {
            string sqlQuery = $@"UPDATE Customers SET IsActive = false WHERE ID = @ID";

            return await DbExecuteAsync<bool>(sqlQuery, new { id });
        }

        public Task<bool> ExistAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await DbQueryAsync<Customer>("SELECT * FROM Customers WHERE IsActive=true");
        }

        public async Task<Customer> GetByIdAsync(object id)
        {
            return await DbQuerySingleAsync<Customer>("SELECT * FROM Customers WHERE Id = @ID", new { id });
        }

        public async Task<(IEnumerable<Customer> Customers, Pagination Pagination)> GetCustomersAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<Customer> customers;
            int recordCount = default;

            ////For PosgreSql
            var query = @"SELECT * FROM Customers
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
                query += " SELECT COUNT(ID) FROM Customers";
                var pagedRows = await DbQueryMultipleAsync<Customer, int>(query, param);

                customers = pagedRows.Data;
                recordCount = pagedRows.RecordCount;
            }
            else
            {
                customers = await DbQueryAsync<Customer>(query, param);
            }

            var metadata = new Pagination
            {
                PageNumber = urlQueryParameters.PageNumber,
                PageSize = urlQueryParameters.PageSize,
                TotalRecords = recordCount

            };

            return (customers, metadata);
        }

        public async Task<bool> UpdateAsync(Customer entity)
        {
            string sqlQuery = $@"UPDATE Customers
                                        SET DisplayName= @DisplayName,
                                        FirstName= @FirstName,
                                        LastName= @LastName,
                                        Notes= @Notes,
                                        Address= @Address,
                                        Mobile1= @Mobile1,
                                        Mobile2= @Mobile2,
                                        BusinessName= @BusinessName,
                                        BusinessAddress= @BusinessAddress,
                                        BusinessMobile1= @BusinessMobile1,
                                        BusinessMobile2= @BusinessMobile2,
                                        CreationAt= @CreationAt,
                                        CreatedBy= @CreatedBy,
                                        ModifiedAt= @ModifiedAt,
                                        ModifiedBy= @ModifiedBy,
                                        IsActive= @IsActive
                                        WHERE Id = @Id";

            return await DbExecuteAsync<bool>(sqlQuery, entity);
        }
    }
}
