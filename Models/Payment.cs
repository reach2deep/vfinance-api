using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public Loan LoadId { get; set; }
        public string PaymentStatus { get; set; }
        public int PaymentDueNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string CollectedBy { get; set; }
        public DateTime CollectedAt { get; set; }
        public string Notes { get; set; }
    }

    public class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Map entities to tables
            builder.ToTable("Payments");

            // Configure Primary Keys            
            builder.HasKey(u => u.Id).HasName("PK_PaymentId");

            // Configure indexes  
            builder.HasIndex(p => p.PaymentDueNumber).IsUnique().HasDatabaseName("Idx_PaymentDueNumber");

            // Configure columns  
            builder.Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        }
    }
}
