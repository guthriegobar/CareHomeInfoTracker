using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CareHomeInfoTracker.Models;

public partial class WeightHistory
{
    public int Id { get; set; }

    public int? ResidentId { get; set; }

    public double? Weight { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Recorded Date")]
    public DateOnly? RecordedDate { get; set; }

    public virtual Resident? Resident { get; set; }
}
