using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Premium
{
    public int PremiumId { get; set; }

    public int ClientPolicyId { get; set; }

    public string DateOfCollection { get; set; } = null!;

    public decimal Penalty { get; set; }

    public virtual ClientPolicy ClientPolicy { get; set; } = null!;
}
