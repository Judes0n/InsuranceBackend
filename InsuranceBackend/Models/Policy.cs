using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InsuranceBackend.Models;

public partial class Policy
{
    public int PolicyId { get; set; }

    public int CompanyId { get; set; }

    public int PolicytypeId { get; set; }

    public string PolicyName { get; set; } = null!;

    public int TimePeriod { get; set; }

    public decimal PolicyAmount { get; set; }

    public StatusEnum Status { get; set; }

    [JsonIgnore]
    public virtual Company Company { get; set; } = null!;
    public virtual ICollection<PolicyTerm> PolicyTerms { get; } = new List<PolicyTerm>();
    [JsonIgnore]
    public virtual PolicyType Policytype { get; set; } = null!;
}
