using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Nominee
{
    public int NomineeId { get; set; }

    public int ClientId { get; set; }

    public string? NomineeName { get; set; }

    public string? Relation { get; set; }

    public string? Address { get; set; }

    public decimal PhoneNum { get; set; }

    public virtual Client? Client { get; set; }
}
