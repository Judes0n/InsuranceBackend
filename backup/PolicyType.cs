using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models.backup;

public partial class PolicyType
{
    public int PolicytypeId { get; set; }

    public string? PolicytypeName { get; set; }

    public virtual ICollection<Policy> Policies { get; } = new List<Policy>();
}
