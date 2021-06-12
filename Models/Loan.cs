using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public string LoanNumber { get; set; }
        public Customer Customer { get; set; }
        public DateTime LoanDate { get; set; }
        public string LoanTerm { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public DateTime PaymentStartDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public Customer ReferredBY { get; set; }
        public DateTime CreationAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool IsActive { get; set; }
    }

    public class LoanEntityConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            // Map entities to tables
            builder.ToTable("Loans");

            // Configure Primary Keys            
            builder.HasKey(u => u.Id).HasName("PK_LoanId");

            // Configure indexes  
            builder.HasIndex(p => p.LoanNumber).IsUnique().HasDatabaseName("Idx_LoanNumber");

            // Configure columns  
            builder.Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        }
    }

}
