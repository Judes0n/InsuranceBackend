using System;
using System.Collections.Generic;

namespace InsuranceBackend.Models;

public partial class Feedback
{
    public int Fid { get; set; }

    public string Feed { get; set; } = null!;
}
