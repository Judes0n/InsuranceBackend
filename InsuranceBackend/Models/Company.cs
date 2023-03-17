using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public int? UserId { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public decimal? PhoneNum { get; set; }

    public string? ProfilePic { get; set; }

    public ActorStatusEnum Status { get; set; }

    public virtual ICollection<Policy> Policies { get; } = new List<Policy>();

    public virtual User? User { get; set; }
}
