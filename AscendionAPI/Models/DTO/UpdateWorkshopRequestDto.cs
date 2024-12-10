﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;

namespace AscendionAPI.Models.Domain;

public class UpdateWorkshopRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [AlphanumericWithSpaces(ErrorMessage = "Name can only contain letters, digits, and spaces")]
    public string Name { get; set; }

    [Required]
    [EnumDataType(typeof(WorkshopCategory), ErrorMessage = "Invalid category for workshop")]
    public string Category { get; set; }

    [Required]
    [MaxLength(2048)]
    [MinLength(20)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public UpdateWorkshopRequestLocationDto Location { get; set; }

    [Required]
    public UpdateWorkshopRequestModesDto Modes { get; set; }
}

public class UpdateWorkshopRequestLocationDto
{
    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }
}

public class UpdateWorkshopRequestModesDto
{
    public bool InPerson { get; set; }
    public bool Online { get; set; }
}