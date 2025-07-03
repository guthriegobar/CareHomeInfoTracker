using System;
using System.Collections.Generic;

namespace CareHomeInfoTracker.Models.TempModels;

public partial class WeightHistory
{
    public int Id { get; set; }

    public int? ResidentId { get; set; }

    public double? Weight { get; set; }

    public DateOnly? RecordedDate { get; set; }

    public virtual Resident? Resident { get; set; }
}
