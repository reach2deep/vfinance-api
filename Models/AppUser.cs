using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        public string UserCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreationAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }

    public class AppUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Map entities to tables
            builder.ToTable("AppUsers");

            // Configure Primary Keys            
            builder.HasKey(u => u.Id).HasName("PK_AppUserId");


            // Configure indexes  
            builder.HasIndex(p => p.UserCode).IsUnique().HasDatabaseName("Idx_AppUserId");
            builder.HasIndex(p => p.Email).IsUnique().HasDatabaseName("Idx_Email");


            // Configure columns  
            builder.Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        }
    }

}
