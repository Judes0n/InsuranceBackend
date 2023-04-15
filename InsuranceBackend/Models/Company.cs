using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;

    public string ProfilePic { get; set; } = null!;

    public ActorStatusEnum Status { get; set; }

    public virtual ICollection<AgentCompany> AgentCompanies { get; } = new List<AgentCompany>();

    public virtual ICollection<Policy> Policies { get; } = new List<Policy>();

    public virtual User User { get; set; } = null!;
}
