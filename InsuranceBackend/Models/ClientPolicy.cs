using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class ClientPolicy
{
    public int ClientPolicyId { get; set; }

    public int ClientId { get; set; }

    public int? PolicyTermId { get; set; }

    public string? StartDate { get; set; }

    public string? ExpDate { get; set; }

    public int? Status { get; set; }

    public int AgentId { get; set; }

    public virtual Agent Agent { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual ICollection<ClientDeath> ClientDeaths { get; } = new List<ClientDeath>();

    public virtual ICollection<Maturity> Maturities { get; } = new List<Maturity>();

    public virtual PolicyTerm? PolicyTerm { get; set; }

    public virtual ICollection<Premium> Premia { get; } = new List<Premium>();
}
