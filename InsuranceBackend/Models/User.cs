using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public UserTypeEnum Type { get; set; }

    public StatusEnum Status { get; set; }

    public virtual ICollection<Agent> Agents { get; } = new List<Agent>();

    public virtual ICollection<Client> Clients { get; } = new List<Client>();

    public virtual ICollection<Company> Companies { get; } = new List<Company>();
}
