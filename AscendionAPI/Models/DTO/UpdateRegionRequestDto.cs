using System;
using System.ComponentModel.DataAnnotations;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Models.DTO;

public class UpdateRegionRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
    [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
    public string Code { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
    public string Name { get; set; }

    public string? RegionImageUrl { get; set; }

    public UpdateRegionRequestDto()
	{
	}

    public UpdateRegionRequestDto( Region region )
    {
        Code = region.Code;
        Name = region.Name;
        RegionImageUrl = region.RegionImageUrl;
    }

    public void UpdateRegion(Region region)
    {
        region.Code = Code;
        region.Name = Name;
        region.RegionImageUrl = RegionImageUrl;
    }
}

