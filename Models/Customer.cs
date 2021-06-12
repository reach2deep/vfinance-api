using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Notes { get; set; }
        public string Address { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessMobile1 { get; set; }
        public string BusinessMobile2 { get; set; }
        public DateTime CreationAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Map entities to tables
            builder.ToTable("Customers");

            // Configure Primary Keys            
            builder.HasKey(u => u.Id).HasName("PK_CustomerId");


            // Configure columns  
            builder.Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        }
    }

}
