using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public int UserId { get; set; }

    public string ClientName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Dob { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ProfilePic { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;

    public string Email { get; set; } = null!;

    public ActorStatusEnum Status { get; set; }

    public virtual ICollection<ClientPolicy> ClientPolicies { get; } = new List<ClientPolicy>();

    public virtual ICollection<Nominee> Nominees { get; } = new List<Nominee>();

    public virtual User User { get; set; } = null!;
}
