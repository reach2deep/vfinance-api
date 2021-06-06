using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vfinance_api.Models;

namespace vfinance_api.DataManager
{
    public class vFinDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        private readonly IConfiguration Configuration;
        private readonly ILogger<vFinDbContext> _logger;

        public vFinDbContext(IConfiguration configuration, ILogger<vFinDbContext> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            try
            {
                _logger.Log(LogLevel.Information, "OnConfiguring");

                string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
                // connect to sqlite database
                options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr));

                _logger.Log(LogLevel.Information, " Completed OnConfiguring");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error when Connecting to Database");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.Log(LogLevel.Information, "OnModelCreating");

            modelBuilder.ApplyConfiguration(new AppUserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentEntityConfiguration());

            _logger.Log(LogLevel.Information, "Completed OnModelCreating");

        }
    }
}
