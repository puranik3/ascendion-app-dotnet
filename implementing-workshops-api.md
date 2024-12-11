# Building the Workshops API using ASP.NET Core 8.0 Web API
- Define `Repositories/IWorkshopRepository.cs`
```cs
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
```
- Inject `ApplicationDbContext` in `Repositories/SqlWorkshopRepository.cs`
```cs
private ApplicationDbContext _db;

public SqlWorkshopRepository(ApplicationDbContext db)
{
    _db = db;
}
```
- Implement methods in `Repositories/SqlWorkshopRepository.cs`
```cs
public async Task<List<Workshop>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null)
{
    // Include("Difficulty") -> type unsafe
    // Include(x => x.Region) -> type safe
    // return await dbContext.Walks.Include("Difficulty").Include(x => x.Region).ToListAsync();
    // return await dbContext.Walks.ToListAsync();

    var query = _db.Workshops.Include("Sessions").AsQueryable();

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
    return await _db.Workshops.Include("Sessions").FirstOrDefaultAsync(w => w.Id == id);
}


public async Task<Workshop> CreateAsync(Workshop workshop)
{
    await _db.Workshops.AddAsync(workshop);
    // on save, the new Walk is added to the DB, and the model's Id filed is also populated with the auto-generated id
    await _db.SaveChangesAsync();

    return workshop;
}

public async Task<Workshop?> UpdateAsync(int id, Workshop workshop)
{
    var existingWorkshop = await _db.Workshops.FirstOrDefaultAsync(w => w.Id == id);

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

    await _db.SaveChangesAsync();

    return existingWorkshop;
}

public async Task<Workshop?> DeleteAsync(int id)
{
    var workshopDomainModel = await _db.Workshops.FirstOrDefaultAsync(w => w.Id == id);

    if (workshopDomainModel == null)
    {
        return null;
    }

    _db.Workshops.Remove(workshopDomainModel);
    await _db.SaveChangesAsync();

    return workshopDomainModel;
}
```
- Make available the `IWorkshopRepository` service as a request-scoped object in the app
```cs
builder.Services.AddScoped<IWorkshopRepository, SqlWorkshopRepository>();
```
- Inject `IWorkshopRepository` service into `Controllers/WorkshopsController.cs`
```cs
private readonly IWorkshopRepository _repo;
private readonly IMapper _mapper;

public WorkshopsController(IWorkshopRepository repo, IMapper mapper)
{
    _repo_ = repo
    _mapper = mapper;
}
```
- Implement the methods in `Controllers/WorkshopsController.cs`
```cs
[HttpGet]
public async Task<IActionResult> GetAll([FromQuery] string? FilterOn, [FromQuery] string? FilterQuery)
{
    var workshopsDomain = await _repo.GetAllAsync(FilterOn, FilterQuery);

    var workshopsDto = _mapper.Map<List<WorkshopDto>>(workshopsDomain);

    return Ok(workshopsDto);
}

[HttpGet]
[Route("{id:int}")]
public async Task<IActionResult> GetById([FromRoute] int id)
{
    var workshopDomain = await _repo.GetByIdAsync(id);

    if (workshopDomain == null)
    {
        return NotFound();
    }

    var workshopDto = _mapper.Map<WorkshopDto>(workshopDomain);

    return Ok(workshopDto);
}

[HttpPost]
public async Task<IActionResult> Create([FromBody] AddWorkshopRequestDto addWorkshopRequestDto)
{
    if (!ModelState.IsValid)
    {
       return BadRequest(ModelState);
    }

    var workshopDomainModel = _mapper.Map<Workshop>(addWorkshopRequestDto);

    workshopDomainModel = await _repo.CreateAsync(workshopDomainModel);

    var workshopDto = _mapper.Map<WorkshopDto>(workshopDomainModel);

    return CreatedAtAction(nameof(GetById), new { id = workshopDto.Id }, workshopDto);
}

[HttpPut]
[Route("{id:int}")]
public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateWorkshopRequestDto updateWorkshopRequestDto)
{
    if (!ModelState.IsValid)
    {
       return BadRequest(ModelState);
    }

    var workshopDomainModel = _mapper.Map<Workshop>(updateWorkshopRequestDto);
    workshopDomainModel = await _repo.UpdateAsync(id, workshopDomainModel);

    if (workshopDomainModel == null)
    {
        return NotFound();
    }

    var workshopDto = _mapper.Map<WorkshopDto>(workshopDomainModel);

    return Ok(workshopDto);
}

[HttpDelete]
[Route("{id:int}")]
public async Task<IActionResult> Delete([FromRoute] int id)
{
    var workshopDomainModel = await _repo.DeleteAsync(id);

    if (workshopDomainModel == null)
    {
        return NotFound();
    }

    // If you'd like, return the deleted Workshop model
    // var workshopDto = new WorkshopDto(workshopDomainModel);
    // return Ok(workshopDto);

    return Ok();
}
```
- Add `Models/DTO/AddWorkshopRequestDto.cs` and `Models/DTO/UpdateWorkshopRequestDto.cs` by copying `Models/Domain/Workshop.cs`. Ensure these new DTOs have validations set up. There is no need to redefine WorkshopCategory - you can reuse the one form the `Workshop` Domain model. Make sure to set their namespace to `AscendionAPI.Models.DTO`
- `Models/DTO/AddWorkshopRequestDto.cs`
```cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Models.DTO;

public class AddWorkshopRequestDto
{
	[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

	[Required(ErrorMessage = "Workshop name is required")]
	[AlphanumericWithSpaces(ErrorMessage = "Name can have only alphanumeric characters and spaces")]
	public string Name { get; set; }

	[Required]
	[EnumDataType(typeof(WorkshopCategory), ErrorMessage = "Workshop category must be one of the allowed types")]
	public string Category { get; set; }

	[Required]
	[MaxLength(2048)]
	[MinLength(20)]
	public string Description { get; set; }

	// navigation property - not a real database property
	public ICollection<Session> Sessions { get; set; }
}
```
- `Models/DTO/UpdateWorkshopRequestDto.cs`
```cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Models.DTO;

public class UpdateWorkshopRequestDto
{
	[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

	[Required(ErrorMessage = "Workshop name is required")]
	[AlphanumericWithSpaces(ErrorMessage = "Name can have only alphanumeric characters and spaces")]
	public string Name { get; set; }

	[Required]
	[EnumDataType(typeof(WorkshopCategory), ErrorMessage = "Workshop category must be one of the allowed types")]
	public string Category { get; set; }

	[Required]
	[MaxLength(2048)]
	[MinLength(20)]
	public string Description { get; set; }

	// navigation property - not a real database property
	public ICollection<Session> Sessions { get; set; }
}
```
- Set up the appropriate mappings in `Mappings/AutoMapperProfiles.cs`.
```cs
CreateMap<Workshop, WorkshopDto>().ReverseMap();
CreateMap<Session, SessionDto>().ReverseMap();

// Add these...
CreateMap<AddWorkshopRequestDto, Workshop>().ReverseMap();
CreateMap<UpdateWorkshopRequestDto, Workshop>().ReverseMap();
```
- You should now be able to perform CRUD operations on the Workshops resource.
- Instead of repeating the logic for BadRequest validation, you can get it done through a custom action filter attribute. Create `CustomActionFilters/ValidateModelAttribute.cs`.
```cs
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AscendionAPI.CustomActionFilters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if( !context.ModelState.IsValid )
        {
            context.Result = new BadRequestResult();
        }
    }
}
```
- Apply this on the Post and Put request actions (`Create()` and `Update()`)
```cs
using AscendionAPI.CustomActionFilters;
```
```cs
[HttpPost]
[ValidateModel]
public async Task<IActionResult> Create([FromBody] AddWorkshopRequestDto addWorkshopRequestDto)
{
    // Not needed anymore...
    // if (!ModelState.IsValid)
    // {
    //     return BadRequest(ModelState);
    // }

    var workshopDomainModel = mapper.Map<Workshop>(addWorkshopRequestDto);
    // rest of code...
    // ...
}
```
```cs
[HttpPut]
[ValidateModel]
public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateWorkshopRequestDto updateWorkshopRequestDto)
{
    // Not needed anymore...
    // if (!ModelState.IsValid)
    // {
    //     return BadRequest(ModelState);
    // }

    var workshopDomainModel = _mapper.Map<Workshop>(updateWorkshopRequestDto);
}
```
- __EXERCISE__: Do similar changes for the Sessions resource.
- Modify `Models/Domain/Workshop.cs` to include the remaining fields of a workshop
```cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;

namespace AscendionAPI.Models.Domain;

public class Workshop
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the key
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [AlphanumericWithSpaces(ErrorMessage = "Name can only contain letters, digits, and spaces")]
    public string Name { get; set; }

    [Required]
    [EnumDataType(typeof(WorkshopCategory), ErrorMessage = "Invalid category for workshop")]
    public string Category { get; set; }

    [Required]
    [MaxLength(2048)]
    [MinLength(20)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public Location Location { get; set; }

    [Required]
    public Modes Modes { get; set; }

    // Navigation properties
    public ICollection<Session> Sessions { get; set; }
}

public class Location
{
    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }
}

public class Modes
{
    public bool InPerson { get; set; }
    public bool Online { get; set; }
}

public enum WorkshopCategory
{
    frontend,
    backend,
    mobile,
    database,
    devops,
    language
}
```
- __NOTE__: Since we use DTOs to transfer data from the client to server, only DTOs need validation. Domain models do not need validation and you can remove the validation attributes form them.
- Update the DTOs as well. The DTOs that transfer data from client to server are `AddWorkshopRequestDto` and `UpdateWorkshopRequest.cs`, and only these 2 models require validations.
- `Models/DTO/WorkshopDto.cs`
```cs
namespace AscendionAPI.Models.DTO;

public class WorkshopDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? ImageUrl { get; set; }

    public LocationDto Location { get; set; }
    public ModesDto Modes { get; set; }

    // Navigation properties
    public ICollection<SessionDto>? Sessions { get; set; }
}

public class LocationDto
{
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}

public class ModesDto
{
    public bool InPerson { get; set; }
    public bool Online { get; set; }
}
```
- `Models/DTO/AddWorkshopRequestDto.cs`
```cs
using System.ComponentModel.DataAnnotations;
using AscendionAPI.Models.DataAnnotations;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Models.DTO;

public class AddWorkshopRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [AlphanumericWithSpaces(ErrorMessage = "Name can only contain letters, digits, and spaces")]
    public string Name { get; set; }

    [Required]
    [EnumDataType(typeof(WorkshopCategory), ErrorMessage = "Invalid category for workshop")]
    public string Category { get; set; }

    [Required]
    [MaxLength(2048)]
    [MinLength(20)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public AddWorkshopRequestLocationDto Location { get; set; }

    [Required]
    public AddWorkshopRequestModesDto Modes { get; set; }
}

public class AddWorkshopRequestLocationDto
{
    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }
}

public class AddWorkshopRequestModesDto
{
    public bool InPerson { get; set; }
    public bool Online { get; set; }
}
```
- `Models/DTO/UpdateWorkshopRequestDto.cs`
```cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AscendionAPI.Models.DataAnnotations;
using AscendionAPI.Models.Domain;

namespace AscendionAPI.Models.DTO;

public class UpdateWorkshopRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [AlphanumericWithSpaces(ErrorMessage = "Name can only contain letters, digits, and spaces")]
    public string Name { get; set; }

    [Required]
    [EnumDataType(typeof(WorkshopCategory), ErrorMessage = "Invalid category for workshop")]
    public string Category { get; set; }

    [Required]
    [MaxLength(2048)]
    [MinLength(20)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public UpdateWorkshopRequestLocationDto Location { get; set; }

    [Required]
    public UpdateWorkshopRequestModesDto Modes { get; set; }
}

public class UpdateWorkshopRequestLocationDto
{
    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string State { get; set; }
}

public class UpdateWorkshopRequestModesDto
{
    public bool InPerson { get; set; }
    public bool Online { get; set; }
}
```
- Set up the appropriate mappings in `Mappings/AutoMapperProfiles.cs`.
```cs
CreateMap<Location, LocationDto>().ReverseMap();
CreateMap<Modes, ModesDto>().ReverseMap();

CreateMap<AddWorkshopRequestLocationDto, Location>().ReverseMap();
CreateMap<AddWorkshopRequestModesDto, Modes>().ReverseMap();

CreateMap<UpdateWorkshopRequestLocationDto, Location>().ReverseMap();
CreateMap<UpdateWorkshopRequestModesDto, Modes>().ReverseMap();
```
- We added fields for `Workshop` model, and we have already been seeded the database with workshops without those field. If we seed new data, then the values for new columns would need to be set to null in these records (which will cause seeding to fail as many of these fields are required). So we will drop the database completely, and start migrations afresh.
- Delete the `Migrations` folder, and drop the database. If using MySQL, drop it by connecting to it using a client like MySQLWorkbench (or something like the MySQL CLI client). If using SQLite, simply delete the `app.db` file in this project.
- Now add/update the following in `Data/ApplicationDbContext.cs`. Within `OnModelCreating()` keep this as is.
```cs
// Define one-to-many relationship between Workshop and Session
modelBuilder.Entity<Workshop>()
    .HasMany(w => w.Sessions)
    .WithOne(s => s.Workshop)
    // .HasForeignKey(s => s.WorkshopId)
    .OnDelete(DeleteBehavior.Cascade); // Cascade delete

```
- We configure `Location` and `Modes` as owned entities so that separate tables are not created for these. Rather the fields of `Location` and `Modes` become columns of the `Workshop` table as configure below.
```cs
// Configure 'Location' as an owned by 'Workshop'
modelBuilder.Entity<Workshop>()
    .OwnsOne(w => w.Location, l =>
    {
        l.Property(loc => loc.Address).HasColumnName("Address");
        l.Property(loc => loc.City).HasColumnName("City");
        l.Property(loc => loc.State).HasColumnName("State");
    });

// Configure 'Modes' as an owned by 'Workshop'
modelBuilder.Entity<Workshop>()
    .OwnsOne(w => w.Modes, l =>
    {
        l.Property(loc => loc.InPerson).HasColumnName("InPerson");
        l.Property(loc => loc.Online).HasColumnName("Online");
    });
```
- Seed data for `Workshops` table. Not how the data for the owned entities are seeded.
```cs
//  Seed data for workshops
var workshops = new List<Workshop>
{
    new Workshop
    {
        Name = "Angular JS Bootcamp",
        Category = "frontend",
        Id = 1,
        Description = "<p><strong>AngularJS</strong> (also written as <strong>Angular.js</strong>) is a JavaScript-based open-source front-end web application framework mainly maintained by Google and by a community of individuals and corporations to address many of the challenges encountered in developing single-page applications.</p><p>It aims to simplify both the development and the testing of such applications by providing a framework for client-side model–view–controller (MVC) and model–view–viewmodel (MVVM) architectures, along with components commonly used in rich Internet applications. (This flexibility has led to the acronym MVW, which stands for \"model-view-whatever\" and may also encompass model–view–presenter and model–view–adapter.)</p>",
        StartDate = new DateTime(2019, 1, 1),
        EndDate = new DateTime(2019, 1, 3),
        StartTime = new TimeOnly(9, 30),
        EndTime = new TimeOnly(13, 30),
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/ca/AngularJS_logo.svg/2000px-AngularJS_logo.svg.png"
    },
    new Workshop
    {
        Name = "React JS Masterclass",
        Category = "frontend",
        Id = 2,
        Description = "<p><strong>React</strong> (also known as <strong>React.js</strong> or <strong>ReactJS</strong>) is a JavaScript library for building user interfaces. It is maintained by Facebook and a community of individual developers and companies.</p><p>React can be used as a base in the development of single-page or mobile applications. Complex React applications usually require the use of additional libraries for state management, routing, and interaction with an API.</p>",
        StartDate = new DateTime(2019, 1, 14),
        EndDate = new DateTime(2019, 1, 16),
        StartTime = new TimeOnly(10, 0),
        EndTime = new TimeOnly(17, 30),
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/React-icon.svg/640px-React-icon.svg.png"
    },
    new Workshop
    {
        Name = "Crash course in MongoDB",
        Category = "database",
        Id = 3,
        Description = "<p><strong>MongoDB</strong> is a cross-platform document-oriented database program. It is issued under the Server Side Public License (SSPL) version 1, which was submitted for certification to the Open Source Initiative but later withdrawn in lieu of SSPL version 2. Classified as a NoSQL database program, MongoDB uses JSON-like documents with schemata. MongoDB is developed by MongoDB Inc.</p><p>MongoDB supports field, range query, and regular expression searches. Queries can return specific fields of documents and also include user-defined JavaScript functions. Queries can also be configured to return a random sample of results of a given size.</p>",
        StartDate = new DateTime(2019, 1, 20),
        EndDate = new DateTime(2019, 1, 22),
        StartTime = new TimeOnly(12, 30),
        EndTime = new TimeOnly(16, 30),
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/32/Mongo-db-logo.png"
    },
};

modelBuilder.Entity<Workshop>().HasData(workshops);

// Seed data for Location (Owned Type)
modelBuilder.Entity<Workshop>()
    .OwnsOne(w => w.Location)
    .HasData(
        new
        {
            WorkshopId = 1,
            Address = "Tata Elxsi, Prestige Shantiniketan",
            City = "Bangalore",
            State = "Karnataka"
        },
        new
        {
            WorkshopId = 2,
            Address = "Tata Elxsi, IT Park",
            City = "Trivandrum",
            State = "Kerala"
        },
        new
        {
            WorkshopId = 3,
            Address = "HCL, Electronic City Phase 1",
            City = "Bangalore",
            State = "Karnataka"
        }
    );

// Seed data for Modes (Owned Type)
modelBuilder.Entity<Workshop>()
    .OwnsOne(w => w.Modes)
    .HasData(
        new
        {
            WorkshopId = 1,
            InPerson = true,
            Online = false
        },
        new
        {
            WorkshopId = 2,
            InPerson = true,
            Online = true
        },
        new
        {
            WorkshopId = 3,
            InPerson = false,
            Online = true
        }
    );
```
- Now perform migration
```
Add-Migration "Initial migration"
update-database
```
- __EXERCISE__: Do similar changes for the Sessions resource by adding the missing fields. Here is data that can help seed the sessions.
```cs
var sessions = new List<Session>
{
    new Session
    {
        Id = 1,
        WorkshopId = 1,
        SequenceId = 1,
        Name = "Introduction to Angular JS",
        Speaker = "John Doe",
        Duration = 1,
        Level = "Basic",
        Abstract = "In this session you will learn about the basics of Angular JS"
    },
    new Session
    {
        Id = 2,
        WorkshopId = 1,
        SequenceId = 2,
        Name = "Scopes in Angular JS",
        Speaker = "John Doe",
        Duration = 0.5,
        Level = "Basic",
        Abstract = "This session will take a closer look at scopes.  Learn what they do, how they do it, and how to get them to do it for you."
    },
    new Session
    {
        Id = 3,
        WorkshopId = 2,
        SequenceId = 1,
        Name = "Introduction to React JS",
        Speaker = "Paul Smith",
        Duration = 0.5,
        Level = "Basic",
        Abstract = "In this session you will learn about the basics of React JS",
    },
    new Session
    {
        Id = 4,
        WorkshopId = 2,
        SequenceId = 2,
        Name = "JSX",
        Speaker = "Paul Smith",
        Duration = 2,
        Level = "Basic",
        Abstract = "Learn how to use JSX to create view and bind data to it",
    },
};
```
- __NOTE__: Add this as a service in `Program.cs` if your JSON object fields are PascalCased and not converted to camelCased automatically. The resultant casing behavior can be overriden at the field lievel using the `[JsonPropertyName]` attribute.
```
// This is a global-setting alternative to using the [JsonPropertyName] on individual properties
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
```