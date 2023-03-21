using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models.backup;

public partial class Maturity
{
    public int MaturityId { get; set; }

    public int? ClientPolicyId { get; set; }

    public string? MaturityDate { get; set; }

    public decimal? ClaimAmount { get; set; }

    public string? StartDate { get; set; }

    public virtual ClientPolicy? ClientPolicy { get; set; }
}
