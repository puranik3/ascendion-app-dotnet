using System;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Repositories;

public interface IWorkshopRepository
{
    Task<List<Workshop>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null);

    Task<Workshop?> GetByIdAsync(int id);

    Task<Workshop> CreateAsync(Workshop workshop);

    Task<Workshop?> UpdateAsync(int id, Workshop workshop);

    Task<Workshop?> DeleteAsync(int id);
}