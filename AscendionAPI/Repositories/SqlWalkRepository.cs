using System;
using AscendionAPI.Data;
using AscendionAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AscendionAPI.Repositories;

public class SqlWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext dbContext;

    public SqlWalkRepository(NZWalksDbContext dbContext)
	{
        this.dbContext = dbContext;
    }

    public async Task<List<Walk>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null)
    {
        // Include("Difficulty") -> type unsafe
        // Include(x => x.Region) -> type safe
        // return await dbContext.Walks.Include("Difficulty").Include(x => x.Region).ToListAsync();
        // return await dbContext.Walks.ToListAsync();

        var query = dbContext.Walks.Include("Difficulty").Include(x => x.Region).AsQueryable();

        if(!string.IsNullOrWhiteSpace( FilterOn ) && !string.IsNullOrWhiteSpace(FilterQuery))
        {
            if(FilterOn.Equals("Name", StringComparison.OrdinalIgnoreCase) )
            {
                //query = query.Where(x => x.Name.Contains(FilterQuery, StringComparison.OrdinalIgnoreCase));
                query = query.Where(x => x.Name.Contains(FilterQuery));
            }
        }

        return await query.ToListAsync();
        
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await dbContext.Walks.Include("Difficulty").Include(x => x.Region).FirstOrDefaultAsync(r => r.Id == id);
    }


    public async Task<Walk> CreateAsync(Walk walk)
    {
        await dbContext.Walks.AddAsync(walk);
        // on save, the new Walk is added to the DB, and the model's Id filed is also populated with the auto-generated id
        await dbContext.SaveChangesAsync();

        return walk;
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalkDomainModel = await dbContext.Walks.FirstOrDefaultAsync(r => r.Id == id);

        if (existingWalkDomainModel == null)
        {
            return null;
        }

        existingWalkDomainModel.Update(walk);
        await dbContext.SaveChangesAsync();

        return existingWalkDomainModel;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var walkDomainModel = await dbContext.Walks.FirstOrDefaultAsync(r => r.Id == id);

        if (walkDomainModel == null)
        {
            return null;
        }

        dbContext.Walks.Remove(walkDomainModel);
        await dbContext.SaveChangesAsync();

        return walkDomainModel;
    }
}