using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Nominee
{
    public int NomineeId { get; set; }

    public int ClientId { get; set; }

    public string NomineeName { get; set; } = null!;

    public string Relation { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<ClientPolicy> ClientPolicies { get; } = new List<ClientPolicy>();
}
