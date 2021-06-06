using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vfinance_api.Dto.Request
{
    public class CreateExpenseRequest
    {
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
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
