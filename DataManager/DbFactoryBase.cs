using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace vfinance_api.DataManager
{
    public abstract class DbFactoryBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<DbFactoryBase> _logger;
        private IConfiguration config;

        internal string DbConnectionString => _config.GetConnectionString("DefaultConnection");

        public DbFactoryBase(IConfiguration config, ILogger<DbFactoryBase> logger)
        {
            _config = config;
            _logger = logger;
        }

        internal IDbConnection DbConnection => new MySqlConnection(DbConnectionString);

        public virtual async Task<IEnumerable<T>> DbQueryAsync<T>(string sql, object parameters = null)
        {
            _logger.LogInformation(sql);
            using IDbConnection dbCon = DbConnection;
            return parameters == null ? await dbCon.QueryAsync<T>(sql) : await dbCon.QueryAsync<T>(sql, parameters);
        }
        public virtual async Task<T> DbQuerySingleAsync<T>(string sql, object parameters)
        {
            _logger.LogInformation(sql);
            using IDbConnection dbCon = DbConnection;
            return await dbCon.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public virtual async Task<bool> DbExecuteAsync<T>(string sql, object parameters)
        {
            _logger.LogInformation(sql);
            using IDbConnection dbCon = DbConnection;
            return await dbCon.ExecuteAsync(sql, parameters) > 0;
        }

        public virtual async Task<bool> DbExecuteScalarAsync(string sql, object parameters)
        {
            _logger.LogInformation(sql);
            using IDbConnection dbCon = DbConnection;
            return await dbCon.ExecuteScalarAsync<bool>(sql, parameters);
        }

        public virtual async Task<T> DbExecuteScalarDynamicAsync<T>(string sql, object parameters = null)
        {
            _logger.LogInformation(sql);
            using IDbConnection dbCon = DbConnection;
            return parameters == null ? await dbCon.ExecuteScalarAsync<T>(sql) : await dbCon.ExecuteScalarAsync<T>(sql, parameters);
        }



        public virtual async Task<(IEnumerable<T> Data, TRecordCount RecordCount)> DbQueryMultipleAsync<T, TRecordCount>(string sql, object parameters = null)
        {
            IEnumerable<T> data = null;
            TRecordCount totalRecords = default;

            using (IDbConnection dbCon = DbConnection)
            {
                using GridReader results = await dbCon.QueryMultipleAsync(sql, parameters);
                data = await results.ReadAsync<T>();
                totalRecords = await results.ReadSingleAsync<TRecordCount>();
            }

            return (data, totalRecords);
        }
    }
}
