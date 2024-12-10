using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;

namespace AscendionAPI.Models.Domain;

public class WorkshopDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? ImageUrl { get; set; }

    public LocationDto Location { get; set; }
    public ModesDto Modes { get; set; }

    // Navigation properties
    public ICollection<SessionDto>? Sessions { get; set; }
}

public class LocationDto
{
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}

public class ModesDto
{
    public bool InPerson { get; set; }
    public bool Online { get; set; }
}