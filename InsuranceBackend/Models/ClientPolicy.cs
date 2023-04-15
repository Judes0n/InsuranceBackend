using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class ClientPolicy
{
    public int ClientPolicyId { get; set; }

    public int ClientId { get; set; }

    public int PolicyTermId { get; set; }

    public int NomineeId { get; set; }

    public string StartDate { get; set; } = null!;

    public string ExpDate { get; set; } = null!;

    public int? Counter { get; set; }

    public ClientPolicyStatusEnum Status { get; set; }

    public string Referral { get; set; } = null!;

    public int AgentId { get; set; }

    public virtual Agent Agent { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<ClientDeath> ClientDeaths { get; } = new List<ClientDeath>();

    public virtual ICollection<Maturity> Maturities { get; } = new List<Maturity>();

    public virtual Nominee Nominee { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual PolicyTerm PolicyTerm { get; set; } = null!;

    public virtual ICollection<Premium> Premia { get; } = new List<Premium>();
}
