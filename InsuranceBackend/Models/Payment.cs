using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int ClientPolicyId { get; set; }

    public int TransactionId { get; set; }

    public string? Time { get; set; }

    public decimal? Amount { get; set; }

    public int? Status { get; set; }

    public virtual ClientPolicy ClientPolicy { get; set; } = null!;
}
