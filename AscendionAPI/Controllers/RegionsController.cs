using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AscendionAPI.Models.Domain;
using AscendionAPI.Models.DTO;
using AscendionAPI.Repositories;
using AscendionAPI.CustomActionFilters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AscendionAPI.Controllers;

// https://localhost:1234/api/regions
[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository regionRepository;
    private readonly IMapper mapper;

    public RegionsController( IRegionRepository regionRepository, IMapper mapper )
    {
        this.regionRepository = regionRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regionsDomain = await regionRepository.GetAllAsync();

        var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

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


        var regionDto = mapper.Map<RegionDto>(regionDomain);

        return Ok(regionDto);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // ModelState.IsValid indicates if it was possible to bind the incoming values from the request to the addRegionRequestDto model correctly and whether any explicitly specified validation rules in AddRegionRequestDto were broken during the model binding process.
        //if (!ModelState.IsValid )
        //{
        //    return BadRequest(ModelState);
        //}

        var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

        regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

        var regionDto = mapper.Map<RegionDto>(regionDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
        regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        var regionDto = mapper.Map<RegionDto>(regionDomainModel);

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

