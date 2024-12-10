using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AscendionAPI.Models.Domain;
using AscendionAPI.Models.DTO;
using AscendionAPI.Repositories;
using AscendionAPI.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AscendionAPI.Controllers;

// https://localhost:1234/api/regions
[Route("api/[controller]")]
[ApiController]
public class RegionsControllerNoAutoMapper : ControllerBase
{
    private readonly IRegionRepository regionRepository;

    public RegionsControllerNoAutoMapper( IRegionRepository regionRepository )
    {
        this.regionRepository = regionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regionsDomain = await regionRepository.GetAllAsync();

        var regionsDto = Helpers.ToDtoList<Region, RegionDto>(regionsDomain, (region) => new RegionDto(region));

        return Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var regionDomain = await regionRepository.GetByIdAsync(id);

        if( regionDomain == null )
        {
            return NotFound();
        }

        var regionDto = new RegionDto(regionDomain);

        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = new Region(addRegionRequestDto);

        regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

        var regionDto = new RegionDto(regionDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        var regionDomainModel = new Region(updateRegionRequestDto);
        regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        var regionDto = new RegionDto(regionDomainModel);

        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var regionDomainModel = await regionRepository.DeleteAsync(id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // If you'd like, return the deleted region model
        // var regionDto = new RegionDto(regionDomainModel);
        // return Ok(regionDto);

        return Ok();
    }
}

