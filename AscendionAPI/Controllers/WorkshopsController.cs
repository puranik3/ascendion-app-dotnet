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
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AscendionAPI.Controllers;

// https://localhost:1234/api/Workshops
[Route("api/[controller]")]
[ApiController]
public class WorkshopsController : ControllerBase
{
    private readonly IWorkshopRepository workshopRepository;
    private readonly IMapper mapper;

    public WorkshopsController(IWorkshopRepository workshopRepository, IMapper mapper)
    {
        this.workshopRepository = workshopRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Reader,Writer")]
    public async Task<IActionResult> GetAll([FromQuery] string? FilterOn, [FromQuery] string? FilterQuery)
    {
        var workshopsDomain = await workshopRepository.GetAllAsync(FilterOn, FilterQuery);

        var workshopsDto = mapper.Map<List<WorkshopDto>>(workshopsDomain);

        return Ok(workshopsDto);
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Roles = "Reader,Writer")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var workshopDomain = await workshopRepository.GetByIdAsync(id);

        if (workshopDomain == null)
        {
            return NotFound();
        }

        var workshopDto = mapper.Map<WorkshopDto>(workshopDomain);

        return Ok(workshopDto);
    }

    [HttpPost]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Create([FromBody] AddWorkshopRequestDto addWorkshopRequestDto)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var workshopDomainModel = mapper.Map<Workshop>(addWorkshopRequestDto);

        workshopDomainModel = await workshopRepository.CreateAsync(workshopDomainModel);

        var workshopDto = mapper.Map<WorkshopDto>(workshopDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = workshopDto.Id }, workshopDto);
    }

    [HttpPut]
    [Route("{id:int}")]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateWorkshopRequestDto updateWorkshopRequestDto)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var workshopDomainModel = mapper.Map<Workshop>(updateWorkshopRequestDto);
        workshopDomainModel = await workshopRepository.UpdateAsync(id, workshopDomainModel);

        if (workshopRepository == null)
        {
            return NotFound();
        }

        var workshopDto = mapper.Map<WorkshopDto>(workshopDomainModel);

        return Ok(workshopDto);
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var workshopDomainModel = await workshopRepository.DeleteAsync(id);

        if (workshopDomainModel == null)
        {
            return NotFound();
        }

        // If you'd like, return the deleted Workshop model
        // var workshopDto = new WorkshopDto(workshopDomainModel);
        // return Ok(workshopDto);

        return Ok();
    }
}