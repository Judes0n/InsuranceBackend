using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public int UserId { get; set; }

    public string? ClientName { get; set; }

    public string? Gender { get; set; }

    public string? Dob { get; set; }

    public string? Address { get; set; }

    public string? ProfilePic { get; set; }

    public decimal? PhoneNum { get; set; }

    public ActorStatusEnum Status { get; set; }

    public virtual ICollection<ClientPolicy> ClientPolicies { get; } = new List<ClientPolicy>();

    public virtual ICollection<Nominee> Nominees { get; } = new List<Nominee>();

    public virtual User? User { get; set; }

}
