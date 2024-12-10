using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AscendionAPI.Controllers;

// IMPORTANT: Controller is for apps that serve both Views and APIs. ControllerBase if for apps that serve only APIs
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    // GET: api/values
    [HttpGet]
    public IActionResult Get()
    {
        var students = new string[] { "John", "Jane", "Mark", "Mary", "Edwin" };

        return Ok(students);
    }

    //// GET api/values/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/values
    //[HttpPost]
    //public void Post([FromBody]string value)
    //{
    //}

    //// PUT api/values/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody]string value)
    //{
    //}

    //// DELETE api/values/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}