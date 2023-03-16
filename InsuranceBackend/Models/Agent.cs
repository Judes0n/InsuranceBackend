using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Agent
{
    public int AgentId { get; set; }

    public int? UserId { get; set; }

    public string AgentName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public decimal? PhoneNum { get; set; }

    public string Dob { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int Grade { get; set; }

    public string? ProfilePic { get; set; }

    public StatusEnum Status { get; set; }

    public virtual ICollection<ClientPolicy> ClientPolicies { get; } = new List<ClientPolicy>();

    public virtual User? User { get; set; }
}
