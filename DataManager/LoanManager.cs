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
    public class LoanManager : DbFactoryBase, ILoanManager
    {
        private readonly ILogger<LoanManager> _logger;
        private readonly vFinDbContext _context;

        public LoanManager(IConfiguration config, ILogger<LoanManager> logger, vFinDbContext context) : base(config, logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<long> CreateAsync(Loan entity)
        {


            entity.IsActive = true;
            entity.CreationAt = DateTime.Now;
            entity.CreatedBy = "Admin";


            _context.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;

            ////string sqlQuery = $@"INSERT INTO Loanrs (ExpenseDate, Category, Amount, Description, CreationAt, 
            ////                            CreatedBy, ModifiedAt, ModifiedBy, IsActive) 
            ////                            VALUES (@ExpenseDate,@Category,@Amount,@Description,@CreationAt,@CreatedBy,@ModifiedAt,@ModifiedBy,@IsActive)";

            //string sqlQuery = $@"INSERT INTO Loans (Id,DisplayName,FirstName,LastName,Notes,Address,Mobile1,Mobile2,BusinessName,BusinessAddress,BusinessMobile1,BusinessMobile2,CreationAt,CreatedBy,ModifiedAt,ModifiedBy,IsActive)
            //                        VALUES (@Id,@DisplayName,@FirstName,@LastName,@Notes,@Address,@Mobile1,@Mobile2,@BusinessName,@BusinessAddress,@BusinessMobile1,@BusinessMobile2,@CreationAt,@CreatedBy,@ModifiedAt,@ModifiedBy,@IsActive)";

            //   return await DbQuerySingleAsync<long>(sqlQuery, entity);
        }


        public async Task<bool> DeleteAsync(object id)
        {
            string sqlQuery = $@"UPDATE Loans SET IsActive = false WHERE ID = @ID";

            return await DbExecuteAsync<bool>(sqlQuery, new { id });
        }

        public Task<bool> ExistAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await DbQueryAsync<Loan>("SELECT * FROM Loans WHERE IsActive=true");
        }

        public async Task<Loan> GetByIdAsync(object id)
        {
            return await DbQuerySingleAsync<Loan>("SELECT * FROM Loans WHERE Id = @ID", new { id });
        }

        public async Task<(IEnumerable<Loan> Loans, Pagination Pagination)> GetLoansAsync(UrlQueryParameters urlQueryParameters)
        {
            IEnumerable<Loan> customers;
            int recordCount = default;

            ////For PosgreSql
            var query = @"SELECT * FROM Loans
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
                query += " SELECT COUNT(ID) FROM Loans";
                var pagedRows = await DbQueryMultipleAsync<Loan, int>(query, param);

                customers = pagedRows.Data;
                recordCount = pagedRows.RecordCount;
            }
            else
            {
                customers = await DbQueryAsync<Loan>(query, param);
            }

            var metadata = new Pagination
            {
                PageNumber = urlQueryParameters.PageNumber,
                PageSize = urlQueryParameters.PageSize,
                TotalRecords = recordCount

            };

            return (customers, metadata);
        }

        public async Task<bool> UpdateAsync(Loan entity)
        {
            string sqlQuery = $@"UPDATE Loans
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
