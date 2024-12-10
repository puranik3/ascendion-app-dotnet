using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AscendionAPI.Models.Domain;
using AscendionAPI.Models.DTO;
using AscendionAPI.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AscendionAPI.Controllers;

// https://localhost:1234/api/regions
[Route("api/[controller]")]
[ApiController]
public class RegionsControllerOld : ControllerBase
{
    private readonly NZWalksDbContext dbContext;

    public RegionsControllerOld( NZWalksDbContext dbContext )
    {
        this.dbContext = dbContext;
    }

    //[HttpGet]
    //public IActionResult GetAll()
    //{
    //    var regions = new List<Region>
    //    {
    //        new Region
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = "Auckland Region",
    //            Code = "AKL",
    //            RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinyrgb&w=1260&h=750&dpr=1"
    //        },
    //        new Region
    //        {
    //            Id = Guid.NewGuid(),
    //            Name = "Wellington Region",
    //            Code = "WLG",
    //            RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinyrgb&w=1260&h=750&dpr=1"
    //        }
    //    };

    //    return Ok(regions);
    //}

    [HttpGet]
    public IActionResult GetAll()
    {
        var regionsDomain = dbContext.Regions.ToList();

        var regionsDto = Helpers.ToDtoList<Region, RegionDto>(regionsDomain, (region) => new RegionDto(region));

        return Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var regionDomain = dbContext.Regions.FirstOrDefault(r => r.Id == id);

        if( regionDomain == null )
        {
            return NotFound();
        }

        var regionDto = new RegionDto(regionDomain);

        return Ok(regionDto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        var regionDomainModel = new Region(addRegionRequestDto);

        dbContext.Regions.Add(regionDomainModel);
        // on save, the new region is added to the DB, and the model's Id filed is also populated with the auto-generated id
        dbContext.SaveChanges();

        var regionDto = new RegionDto(regionDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        var regionDomainModel = dbContext.Regions.FirstOrDefault(r => r.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        updateRegionRequestDto.UpdateRegion(regionDomainModel);
        dbContext.SaveChanges();

        var regionDto = new RegionDto(regionDomainModel);

        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var regionDomainModel = dbContext.Regions.FirstOrDefault(r => r.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        dbContext.Regions.Remove(regionDomainModel);
        dbContext.SaveChanges();

        return Ok();
    }
}

