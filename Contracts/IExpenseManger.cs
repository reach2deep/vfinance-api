using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vfinance_api.DataManager;
using vfinance_api.Helper;
using vfinance_api.Models;

namespace vfinance_api.Contracts
{
    public interface IExpenseManager : IRepository<Expense>
    {
        Task<(IEnumerable<Expense> Persons, Pagination Pagination)> GetPersonsAsync(UrlQueryParameters urlQueryParameters);

        //Add more class specific methods here when neccessary
    }
}
