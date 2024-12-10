using System;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Repositories;

public interface IWalkRepository
{
    Task<List<Walk>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null);

    Task<Walk?> GetByIdAsync(Guid id);

    Task<Walk> CreateAsync(Walk walk);

    Task<Walk?> UpdateAsync(Guid id, Walk walk);

    Task<Walk?> DeleteAsync(Guid id);
}