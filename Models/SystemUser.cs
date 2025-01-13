using System;
using System.Collections.Generic;

namespace CareHomeInfoTracker.Models;

public partial class SystemUser
{
    public string Id { get; set; } = null!;

    public string? Password { get; set; }

    public string? Role { get; set; }

    public string? Email { get; set; }
}
