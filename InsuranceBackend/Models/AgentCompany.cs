using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class AgentCompany
{
    public int Id { get; set; }

    public int AgentId { get; set; }

    public int CompanyId { get; set; }

    public string Referral { get; set; } = null!;

    public StatusEnum Status { get; set; }

    public virtual Agent Agent { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;
}
