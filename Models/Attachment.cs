using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreationAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class AttachmentEntityConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            // Map entities to tables
            builder.ToTable("Attachments");

            // Configure Primary Keys            
            builder.HasKey(u => u.Id).HasName("PK_AttachmentId");


            // Configure indexes  
            builder.HasIndex(p => p.FileName).IsUnique().HasDatabaseName("Idx_FileName");
            builder.HasIndex(p => p.FilePath).IsUnique().HasDatabaseName("Idx_FilePath");


            // Configure columns  
            builder.Property(ug => ug.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
        }
    }

}
