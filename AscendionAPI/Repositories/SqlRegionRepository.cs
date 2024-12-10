using System;
using AscendionAPI.Data;
using AscendionAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AscendionAPI.Repositories;

public class SqlRegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext dbContext;

    public SqlRegionRepository(NZWalksDbContext dbContext)
	{
        this.dbContext = dbContext;
    }

    public async Task<List<Region>> GetAllAsync()
    {
        return await dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(Guid id)
    {
        return await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
    }


    public async Task<Region> CreateAsync(Region region)
    {
        await dbContext.Regions.AddAsync(region);
        // on save, the new region is added to the DB, and the model's Id filed is also populated with the auto-generated id
        await dbContext.SaveChangesAsync();

        return region;
    }

    public async Task<Region?> UpdateAsync(Guid id, Region region)
    {
        var existingRegionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (existingRegionDomainModel == null)
        {
            return null;
        }

        existingRegionDomainModel.Update(region);
        await dbContext.SaveChangesAsync();

        return existingRegionDomainModel;
    }

    public async Task<Region?> DeleteAsync(Guid id)
    {
        var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (regionDomainModel == null)
        {
            return null;
        }

        dbContext.Regions.Remove(regionDomainModel);
        await dbContext.SaveChangesAsync();

        return regionDomainModel;
    }
}