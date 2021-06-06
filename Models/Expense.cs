using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreationAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool IsActive { get; set; }
    }

    public class ExpenseEntityConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            // Map entities to tables
            builder.ToTable("Expenses");

            // Configure Primary Keys            
            builder.HasKey(u => u.Id).HasName("PK_ExpenseId");


            // Configure indexes  
            builder.HasIndex(p => p.Category).IsUnique().HasDatabaseName("Idx_Category");
            builder.HasIndex(p => p.Amount).IsUnique().HasDatabaseName("Idx_Amount");


            // Configure columns  
            builder.Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        }
    }

}
