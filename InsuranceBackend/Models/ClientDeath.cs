using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class ClientDeath
{
    public int ClientDeathId { get; set; }

    public int ClientPolicyId { get; set; }

    public string Dod { get; set; } = null!;

    public string StartDate { get; set; } = null!;

    public decimal ClaimAmount { get; set; }

    public virtual ClientPolicy ClientPolicy { get; set; } = null!;
}
