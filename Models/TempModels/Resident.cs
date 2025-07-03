using System;
using System.Collections.Generic;

namespace CareHomeInfoTracker.Models.TempModels;

public partial class Resident
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? RoomNum { get; set; }

    public string? BedNum { get; set; }

    public string? ImgLocation { get; set; }

    public virtual ICollection<WeightHistory> WeightHistories { get; set; } = new List<WeightHistory>();
}
