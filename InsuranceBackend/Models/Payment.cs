using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int ClientPolicyId { get; set; }

    public string? TransactionId { get; set; }

    public string Time { get; set; } = null!;

    public decimal Amount { get; set; }

    public PaymentStatusEnum Status { get; set; }

    public virtual ClientPolicy ClientPolicy { get; set; } = null!;
}
