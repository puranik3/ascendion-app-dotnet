using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;

namespace AscendionAPI.Models.Domain;

public class Session
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the key
    public int Id { get; set; }

	[ForeignKey("Workshop")]
	public int WorkshopId { get; set; }

    [Range(1, int.MaxValue)]
    public int SequenceId { get; set; }

	[Required]
    [AlphanumericWithSpaces(ErrorMessage = "Name can only contain letters, digits, and spaces")]
    public string Name { get; set; }

	[Required]
    [AlphanumericWithSpaces(ErrorMessage = "Name can only contain letters, digits, and spaces")]
    public string Speaker { get; set; }

	[Required]
	[Range(0.0, 128.0)]
	public double Duration { get; set; }

    [Required]
    [EnumDataType(typeof(SessionLevel), ErrorMessage = "Invalid level for session")]
    public string Level { get; set; }

    [Required]
    [MinLength(20)]
    [MaxLength(2048)]
    public string Abstract { get; set; }

	public int? UpvoteCount { get; set; } = 0;

	// Navigation properties
	public Workshop Workshop { get; set; } // Navigation Property
}

public enum SessionLevel
{
    Basic,
    Intermediate,
    Advanced
}