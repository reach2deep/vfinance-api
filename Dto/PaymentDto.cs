using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vfinance_api.Models;

namespace vfinance_api.Dto
{
    public class PaymentDto
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
}
