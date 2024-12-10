using System;
using AscendionAPI.Data;
using AscendionAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AscendionAPI.Repositories;

public class InMemoryRegionRepository : IRegionRepository
{
    List<Region> Regions = new List<Region>
    {
        new Region
        {
            Id = new Guid("08dd0518-9303-48d8-88d2-aef997514625"),
            Code = "AKL",
            Name = "Auckland Region",
            RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinyrgb&w=1260&h=750&dpr=1"
        },
        new Region
        {
            Id = new Guid("08dd051d-778d-4d66-8c18-010cf5990cf6"),
            Code = "WLG",
            Name = "Wellington Region",
            RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinyrgb&w=1260&h=750&dpr=1"
        }
    };

    public async Task<List<Region>> GetAllAsync()
    {
        return Regions;
    }

    public Task<Region?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Region> CreateAsync(Region region)
    {
        throw new NotImplementedException();
    }

    public Task<Region?> UpdateAsync(Guid id, Region region)
    {
        throw new NotImplementedException();
    }

    public Task<Region?> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}