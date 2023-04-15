using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Agent
{
    public int AgentId { get; set; }

    public int UserId { get; set; }

    public string AgentName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;

    public string Dob { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int Grade { get; set; }

    public string ProfilePic { get; set; } = null!;

    public ActorStatusEnum Status { get; set; }

    public virtual ICollection<AgentCompany> AgentCompanies { get; } = new List<AgentCompany>();

    public virtual ICollection<ClientPolicy> ClientPolicies { get; } = new List<ClientPolicy>();

    public virtual User User { get; set; } = null!;
}
