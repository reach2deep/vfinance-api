using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Dto.Request
{
    public class ExpenseResponse
    {
        public long Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreationAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AttachmentRequest>? Attachments { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateExpenseRequest
    {
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AttachmentRequest> Attachments { get; set; }
    }
    public class AttachmentRequest
    {
        public string FileName { get; set; }
        public Byte fileArray { get; set; }
    }
    public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
    {
        public CreateExpenseRequestValidator()
        {
            RuleFor(o => o.ExpenseDate).NotEmpty();
            RuleFor(o => o.Category).NotEmpty();
            RuleFor(o => o.Amount).NotEmpty();
        }
    }
}
