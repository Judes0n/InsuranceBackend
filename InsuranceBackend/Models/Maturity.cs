using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Maturity
{
    public int MaturityId { get; set; }

    public int ClientPolicyId { get; set; }

    public string MaturityDate { get; set; } = null!;

    public decimal ClaimAmount { get; set; }

    public string StartDate { get; set; } = null!;

    public virtual ClientPolicy ClientPolicy { get; set; } = null!;
}
