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

// https://localhost:1234/api/Walks
[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly IWalkRepository walkRepository;
    private readonly IMapper mapper;

    public WalksController(IWalkRepository walkRepository, IMapper mapper)
    {
        this.walkRepository = walkRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? FilterOn, [FromQuery] string? FilterQuery)
    {
        var walksDomain = await walkRepository.GetAllAsync(FilterOn, FilterQuery);

        var walksDto = mapper.Map<List<WalkDto>>(walksDomain);

        return Ok(walksDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomain = await walkRepository.GetByIdAsync(id);

        if (walkDomain == null)
        {
            return NotFound();
        }

        var walkDto = mapper.Map<WalkDto>(walkDomain);

        return Ok(walkDto);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

        walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

        var walkDto = mapper.Map<WalkDto>(walkDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
        walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

        if (walkDomainModel == null)
        {
            return NotFound();
        }

        var walkDto = mapper.Map<WalkDto>(walkDomainModel);

        return Ok(walkDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var walkDomainModel = await walkRepository.DeleteAsync(id);

        if (walkDomainModel == null)
        {
            return NotFound();
        }

        // If you'd like, return the deleted Walk model
        // var walkDto = new WalkDto(walkDomainModel);
        // return Ok(walkDto);

        return Ok();
    }
}