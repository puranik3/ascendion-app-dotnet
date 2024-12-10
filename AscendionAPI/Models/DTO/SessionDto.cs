using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;

namespace AscendionAPI.Models.Domain;

public class SessionDto
{
    public int Id { get; set; }
	public int WorkshopId { get; set; }
    public int SequenceId { get; set; }
    public string Name { get; set; }
    public string Speaker { get; set; }
	public double Duration { get; set; }
    public string Level { get; set; }
    public string Abstract { get; set; }
	public int? UpvoteCount { get; set; } = 0;

	// Navigation properties
	public Workshop? WorkshopDto { get; set; } // Navigation Property
}