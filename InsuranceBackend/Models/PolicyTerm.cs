using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class PolicyTerm
{
    public int PolicyTermId { get; set; }

    public int PolicyId { get; set; }

    public int Period { get; set; }

    public int Terms { get; set; }

    public decimal PremiumAmount { get; set; }

    public virtual ICollection<ClientPolicy> ClientPolicies { get; } = new List<ClientPolicy>();

    public virtual Policy Policy { get; set; } = null!;
}
