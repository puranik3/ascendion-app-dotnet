using System;
using AscendionAPI.Models.DTO;

namespace AscendionAPI.Models.Domain;

public class Region
{
	public Guid Id { get; set; }
	public string Code { get; set; }
	public string Name { get; set; }
	public string? RegionImageUrl { get; set; }

    public Region()
    {

    }

    public Region(AddRegionRequestDto region)
    {
        Code = region.Code;
        Name = region.Name;
        RegionImageUrl = region.RegionImageUrl;
    }

    public Region(UpdateRegionRequestDto region)
    {
        Code = region.Code;
        Name = region.Name;
        RegionImageUrl = region.RegionImageUrl;
    }

    public void Update(Region region)
    {
        Code = region.Code;
        Name = region.Name;
        RegionImageUrl = region.RegionImageUrl;
    }
}