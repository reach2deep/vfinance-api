using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vfinance_api.Models;

namespace vfinance_api.Dto
{
    public class CustomerDto
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
}
