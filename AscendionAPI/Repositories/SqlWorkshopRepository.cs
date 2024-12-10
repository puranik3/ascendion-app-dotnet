using System;
using AscendionAPI.Data;
using AscendionAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AscendionAPI.Repositories;

public class SqlWorkshopRepository : IWorkshopRepository
{
    private readonly NZWalksDbContext dbContext;

    public SqlWorkshopRepository(NZWalksDbContext dbContext)
	{
        this.dbContext = dbContext;
    }

    public async Task<List<Workshop>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null)
    {
        // Include("Difficulty") -> type unsafe
        // Include(x => x.Region) -> type safe
        // return await dbContext.Walks.Include("Difficulty").Include(x => x.Region).ToListAsync();
        // return await dbContext.Walks.ToListAsync();

        var query = dbContext.Workshops.Include("Sessions").AsQueryable();

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

    public async Task<Workshop?> GetByIdAsync(int id)
    {
        return await dbContext.Workshops.Include("Sessions").FirstOrDefaultAsync(w => w.Id == id);
    }


    public async Task<Workshop> CreateAsync(Workshop workshop)
    {
        await dbContext.Workshops.AddAsync(workshop);
        // on save, the new Walk is added to the DB, and the model's Id filed is also populated with the auto-generated id
        await dbContext.SaveChangesAsync();

        return workshop;
    }

    public async Task<Workshop?> UpdateAsync(int id, Workshop workshop)
    {
        var existingWorkshop = await dbContext.Workshops.FirstOrDefaultAsync(w => w.Id == id);

        if (existingWorkshop == null)
        {
            return null;
        }

        //existingWorkshop.Update(workshop);

        existingWorkshop.Name = workshop.Name;
        existingWorkshop.Category = workshop.Category;
        existingWorkshop.Description = workshop.Description;
        existingWorkshop.StartDate = workshop.StartDate;
        existingWorkshop.EndDate = workshop.EndDate;
        existingWorkshop.StartTime = workshop.StartTime;
        existingWorkshop.EndTime= workshop.EndTime;
        existingWorkshop.ImageUrl = workshop.ImageUrl;
        existingWorkshop.Location = workshop.Location;
        existingWorkshop.Modes = workshop.Modes;

        await dbContext.SaveChangesAsync();

        return existingWorkshop;
    }

    public async Task<Workshop?> DeleteAsync(int id)
    {
        var workshopDomainModel = await dbContext.Workshops.FirstOrDefaultAsync(w => w.Id == id);

        if (workshopDomainModel == null)
        {
            return null;
        }

        dbContext.Workshops.Remove(workshopDomainModel);
        await dbContext.SaveChangesAsync();

        return workshopDomainModel;
    }
}