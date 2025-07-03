using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHomeInfoTracker.Models;

public partial class Resident
{
    public int Id { get; set; }

    [Display(Name = "First Name")]
    [Required]
    public string? FirstName { get; set; }

    [Display(Name = "Middle Name")]
    public string? MiddleName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }
    
    [Display(Name = "Room No")]
    [Required]
    public string? RoomNum { get; set; }
    
    [Display(Name = "Bed No")]
    [Required]
    public string? BedNum { get; set; }

    [NotMapped]
    public IFormFile? ResImage { get; set; }
    public string? ImgLocation { get; set; }

    public virtual ICollection<WeightHistory> WeightHistories { get; set; } = new List<WeightHistory>();
}
