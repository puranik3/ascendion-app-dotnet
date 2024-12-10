## Database Migration commands
- First setup `Data/ApplicationDbContext`, then the connection string in `appsettings.Development.json`.
- On Windows, from NuGet console (Tools -> NuGet package manager console) run this to create the database. Make sure to select the __Default Project__ from the dropdown if your Solution has multiple projects.
```
update-database
```
On Mac, run this instead from the terminal opened in the project folder
```
dotnet ef database update --verbose
```
- If EntityFramework Tools are not installed on Mac, you can do so using
```
dotnet tool install --global dotnet-ef
```
- Make sure the tools path is in the system PATH.
- Next add `public DBSet<Category> { get; set; }` Category to `ApplicationDbContext`.
- Migrate the table
    - On Windows run from the NuGet console
    ```
    Add-Migration AddCategoryTableToDb
    ```
    - On Mac run this instead
    ```
    dotnet ef migrations add AddCategoryTableToDb
    ```
- You will find a `Migrations` folder created with some files. This helps set up the DB on a another environment when the app is deployed.
- Now update the database once again
```
dotnet ef database update --verbose
```

## ASP.NET Core MVC
- Since June 2016
- Now we use .NET 8.0 or .Net 9.0
- Cross-platform
- Fast and open-source
- Built-in Dependency Injection
- Easily updated
- Cloud friendly
- Performant

## Tools needed
- .NET 8.0
    - https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- MySQL 8.4.3 LTS / 8.0.40
    - https://dev.mysql.com/downloads/mysql/
    or
    - https://dev.mysql.com/downloads/installer/
- MySQL Workbench
    - https://dev.mysql.com/downloads/workbench/
- SQLite (This is the CLI client and is optional when working with SQLite DB)
    - https://www.sqlite.org/cli.html

## Setup
- Create Project -> ASP.NET Core Web App (MVC)
    - Authentication type: None
- Add to source control -> Fill in details and "Create and Push"
- Launch the project
- Right click on Project -> "Edit Project File" to view the project file
- Understanding the project structure
    - Dependencies -> `libraries/packages/other projects`
    - Properties -> launch settings
        - you can toggle between the profiles when launching the app
        - port can be controlled
        - environment variables can be set
    - `wwwroot` -> static content
        - css, js, libraries, font files, images
    - MVC (see below)
    - `appsettings.json`
        - based on `launchsettings.json` `ASPNETCORE_ENVIRONMENT` different versions of th file are selected (`appsettings.development.json`, `appsettings.production.json` etc.)
        - connection strings
    - Program.cs (in old verions of .NET we had `Program.cs` and `startup.cs`)
        - Configures services for DI
        - Configures request pipeline (middleware chain)
- MVC architecture
    - Overview of MVC architecture
- Routing
    - In MVC app, the typical route is `/{Controller}/{Action}, /{Controller}/{Action}/{Id}`
- MVC folders
    - Views folder has folders for every controller
    - Controller can use one or more models to fetch data and pass onto views
    - default route controller is `Home/Index`
        - this returns `View()`
            - so the view that is returned is based on the name of the controller method name
            - thus view is `Home/Index.cshtml`
        - if we return `View("Name")` then view is `Home/Name.cshtml` instead
- In Shared we have `_Layout.cshtml` page
    - `@RenderBody()` is the view returned by the controller
    - Add CSS and JS here
- In Shared we have also have `_ValidateScriptsPartial.cshtml`

## Steps
- Models -> Right click and add class -> `Category`
- Create properties
```cs
public int Id { get; set; }
public String Name { get; set; }
public int DisplayOrder { get; set; }
```
- Make Id the Key. The `[Key]` Attribute can actually skipped if the name if `Id` or `CategoryId` (Table name followed by `Id`).
- Also mark the `Required` fields.
```cs
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
```
```cs
[Key]
public int Id { get; set; }

[Required]
public String Name { get; set; }

public int DisplayOrder { get; set; }
```
- If using SQLite then an auto-incremented Id is needed like so
```cs
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the key
public int Id { get; set; }

[Required]
public String Name { get; set; }

public int DisplayOrder { get; set; }

```
- Setting up Connection string
    - appsettings.json / appsettings.Development.json
```json
"ConnectionStrings": {
    "MySQLConnection": "Server=127.0.0.1;Database=WorkshopsDB;User=username;Password=password;Port=3306;"
},
```
- Right click on Project -> Manage NuGet packages
    - Microsoft.EntityFrameworkCore 8.0.2
    - Microsoft.EntityFrameworkCore.Tools 8.0.2
    - Pomelo.EntityFrameworkCore.MySQL 8.0.2
    - __Note__: Entries are added in the project file
- If using SQLite then install this instead of MySQL 8.0.2
    - Microsoft.EntityFrameworkCore.Sqlite 8.0.2
- Create a class under `Data/ApplicationDbContext.cs`
```cs
using System;
using Microsoft.EntityFrameworkCore;

namespace AscendionApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {

    }
}
```
- Register `ApplicationDbContext` in `Program.cs`
```cs
using AscendionApp.Data;
```
```cs
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add this -> Configure DB service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySQLConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection"))
    ));

var app = builder.Build();
```
- If using SQLite, set this instead
```cs
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add this -> Configure DB service -> Add DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

var app = builder.Build();
```

### Create the database and table
- From NuGet console run this to create the database
```
update-database
```
On Mac run this instead from the terminal opened in the project folder
```
dotnet ef database update --verbose
```
- If EntityFramework Tools are not installed on Mac, you can do so using
```
dotnet tool install --global dotnet-ef
```
- Make sure the tools path is in the system `PATH`.

- Create `Category` Table
- Add DbSet<Category> for Categories in `ApplicationDbContext.cs`
```cs
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
{

}

// Add this line
public DbSet<Category> Categories { get; set; }
```
- Migrate the table
- On Windows run from the NuGet console
```
Add-Migration AddCategoryTableToDb
```
- On Mac run this instead
```
dotnet ef migrations add AddCategoryTableToDb
```
- You will find a `Migrations` folder created with some files (`AddCategoryTableToDb.cs`). This helps set up the DB on a another environment when the app is deployed.
- Now update the database once again
- On Windows
```
update-database
```
- On Mac
```
dotnet ef database update --verbose
```
- Add Category Controller
- Add a Controller class - `CategoryController`
- Basic setup
```cs
using Microsoft.AspNetCore.Mvc;

namespace AscendionApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
```
- Add `Views/Category/Index.cshtml` (right click and add view - empty view)
```cshtml
<h1>Category List</h1>
```
- Add Category Link in header in `_Layout.cshtml`

```cs
<li class="nav-item">
    <a class="nav-link" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
</li>

<!-- Add this -->
<li class="nav-item">
    <a class="nav-link" asp-area="" asp-controller="Category" asp-action="Index">Category List</a>
</li>

<li class="nav-item">
    <a class="nav-link" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
</li>
```
- Seed Category table
- We can seed data in `ApplicationDbContext.cs` using `ModelBuilder`
```cs
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) {

}

public DbSet<Category> Categories { get; set; }

// Add this
protected override void OnModelCreating( ModelBuilder modelBuilder )
{
    modelBuilder.Entity<Category>().HasData(
        new Category
        {
            Id = 1,
            Name = "Action",
            DisplayOrder = 1
        },
        new Category
        {
            Id = 2,
            Name = "SciFi",
            DisplayOrder = 2
        },
        new Category
        {
            Id = 3,
            Name = "History",
            DisplayOrder = 3
        }
    );
}
```
- Migrate the data
- On Windows run from the NuGet console
```
Add-Migration SeedCategoryTable
update-database
```

- On Mac run this instead
```
dotnet ef migrations add SeedCategoryTable
dotnet ef database update --verbose
```
- Get all categories
- Set the following up in `CategoryController.cs` that fetches categories from the table and passing on the data to the view
```cs
using AscendionApp.Data;
using AscendionApp.Models;
```
```cs
// Add a field
private readonly ApplicationDbContext _db;

// Inject `ApplicationDbContext` through constructor DI
public CategoryController(ApplicationDbContext db)
{
    _db = db;
}

// GET: /<controller>/
public IActionResult Index()
{
    // Get the list of categories
    List<Category> categories = _db.Categories.ToList();
    return View(categories);
}
```
- Enable "Hot Reload on File Save" option and "Restart Application". The server automatically restarts on UI file changes.
- In `Category/Index.cshtml`
```cs
@model List<Category>

<div class="row mb-3">
    <div class="col-12">
        <h1>Categories</h1>
    </div>
</div>

<table class="table table-bordered table-striped">
    <thead>
        <tr>
            <th>Name</th>
            <th>Display order</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var category in Model.OrderBy(c => c.DisplayOrder))
        {
            <tr>
                <td>@category.Name</td>
                <td>@category.DisplayOrder</td>
            </tr>
        }
    </tbody>
</table>
```
- Theming
- From the Bootswatch site download `bootstrap.lux.min.css` and add it to the `wwwroot/lib/bootstrap/dist/css` folder. Include it in the head section of `_Layout.cshtml`. Also include bootstrap icons.
```cshtml
<link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.lux.min.css" />
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
```
- Make chages to the `nav` styles like so
```cshtml
<nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-primary border-bottom box-shadow mb-3">
```
- and changes to the links like so (sample link shown below)
```
<a class="nav-link" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
```
- and change the footer like so
```cshtml
<footer class="border-top footer bg-light text-center">
    <div class="container">
        <div>
            Made with <i class="bi bi-heart-fill"></i> by John Doe
        </div>
    </div>
</footer>
```
- Modify Category List page
```cshtml
<div class="row mb-3">
    <div class="col-6">
        <h1>Categories</h1>
    </div>
    <div class="col-6 text-end">
        <a asp-controller="" asp-action="" class="btn btn-dark">
            <i class="bi bi-plus-circle"></i>
            Create Category
        </a>
    </div>
</div>
```
- Add the UI for the Create Category page
- `CategoryController.cs` - Add action method
```cs
// GET: /<controller>/Create
public IActionResult Create()
{
    return View();
}
```
- Add the link to a new page in `Index.cshtml`
```cshtml
<a asp-controller="Category" asp-action="Create" class="btn btn-dark">
    <i class="bi bi-plus-circle"></i>
    Create Category
</a>
```
- `Views/Category/Create.cshtml` - Create it. Since we do not pass a model from the controller a new object is created for the model! We add `asp-for` input tag helper to associate the input and label with model properties. The label text is auto-populated.
```cshtml
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@model Category

<div class="row mb-2">
    <h1>Create Category</h1>
    <hr />
</div>

<!-- asp-controller and asp-action can also be set on the form tag, but we can omit it as our GET and POST access/Category/Create, although through different HTTP methods -->
<form method="post">
    <div class="row mb-2 p-1">
        <label asp-for="Name"></label>
        <input type="text" class="form-control" asp-for="Name" />
    </div>
    <div class="row mb-2 p-1">
        <label asp-for="DisplayOrder"></label>
        <input type="text" class="form-control" asp-for="DisplayOrder" />
    </div>
    <div class="row mb-2">
        <div class="col-6 col-md-auto">
            <button type="submit" class="btn btn-primary w-100">Create</button>
        </div>
        <div class="col-6 col-md-auto">
            <a asp-controller="Category" asp-action="Index" class="btn btn-secondary border w-100">Back to List</a>
        </div>
    </div>
</form>
```
- Set data annotations in `Models/Category.cs`. The label cab be updated with `DisplayName` attribute.
```cs
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DisplayName("Category Name")]
    public String Name { get; set; }

    [Required]
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}
```
- Add a Create (post request) Action handler in `CategoryController.cs`
```cs
// POST: /<controller>/Create
[HttpPost]
public IActionResult Create(Category newCategory)
{
    _db.Categories.Add(newCategory);
    _db.SaveChanges();
    return RedirectToAction("Index"); // (action, controller) is an overload
}
```
- Add validations
```cs
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Category Name is required")]
    [StringLength(24, ErrorMessage = "Category Name cannot exceed 24 characters")]
    [DisplayName("Category Name")]
    public String Name { get; set; }

    [Required(ErrorMessage = "Display Order is required")]
    [Range(1, 100, ErrorMessage = "Display Order must be between 1 - 100")]
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}
```
- Update the controller to take care of validations.Note that `TempData` is of type `Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary`. It is data that is available only to the next rendered view.
```cs
// ModelState.IsValid indicates if it was possible to bind the incoming values from the request to the newCategory model correctly and whether any explicitly specified validation rules for Category were broken during the model binding process.
if (ModelState.IsValid)
{
    _db.Categories.Add(newCategory);
    _db.SaveChanges();
    TempData["success"] = "Created a new category";
    return RedirectToAction("Index"); // (action, controller) is an overload
}

TempData["error"] = "Unable to create a new category";
return View(newCategory);
```
- Add tag helpers for validation. We will add server-side validations first.
```cshtml
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@model Category

<div class="row mb-2">
    <h1>Create Category</h1>
    <hr />
</div>

<!-- Add this -->
@if (TempData["Message"] != null)
{
    <p>@TempData["Message"]</p>
}

<form method="post">
    <div class="row mb-2 p-1">
        <label asp-for="Name"></label>
        <input type="text" class="form-control" asp-for="Name" />

        <!-- Add this -->
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    <div class="row mb-2 p-1">
        <label asp-for="DisplayOrder"></label>
        <input type="text" class="form-control" asp-for="DisplayOrder" />

        <!-- Add this -->
        <span asp-validation-for="DisplayOrder" class="text-danger"></span>
    </div>
    <div class="row mb-2">
        <div class="col-6 col-md-auto">
            <button type="submit" class="btn btn-primary w-100">Create</button>
        </div>

        <!-- Add this for going back to the Index page -->
        <div class="col-6 col-md-auto">
            <a asp-controller="Category" asp-action="Index" class="btn btn-secondary border w-100">Back to List</a>
        </div>
    </div>
</form>
```
- Custom validations. In `CategoryController.cs`,
```cs
public IActionResult Create(Category newCategory)
{
    /* Add this custom validation code */
    if(newCategory.Name == newCategory.DisplayOrder.ToString())
    {
        // "" makes this a model-only error. Instead of "", we can add a field name as well -> this makes it a field error, instead of a model-only error
        ModelState.AddModelError("", "Category Name and Display Order cannot be the same");
    }

    // ModelState.IsValid indicates if it was possible to bind the incoming values from the request to the newCategory model correctly and whether any explicitly specified validation rules for Category were broken during the model binding process.
    if (ModelState.IsValid)
    {
        _db.Categories.Add(newCategory);
        _db.SaveChanges();
        TempData["success"] = "Created a new category";
        return RedirectToAction("Index"); // (action, controller) is an overload
    }

    TempData["error"] = "Unable to create a new category";
    return View(newCategory);
}
```
- Add this on top of the form in `Create.cshtml`
- asp-validation-summary="ModelOnly" (model-level error) or asp-validation-summary="All" (includes custom property validation). It can even be set to "None"
```cshtml
<div asp-validation-summary="All" class="text-danger"></div>
```
- Client-side validations. Add jquery validations library that ASP.NET Core (Razor) uses, towards the end of the `Create.cshtml`. It can even be added to `_Layout.cshtml` if most of our pages have forms that require client-side validations.
```cshtml
@* This partial include is needed to enable client-side validations *@
@section Scripts {
    @{
        <partial name="_ValidationScriptsPartial" />
    }
}
```
- Now with invalid input, the form cannot be submitted.
- Add `Edit` and `Delete` buttons in `Index.cshtml`
```cshtml
<table class="table table-bordered table-striped">
    <thead>
        <tr>
            <th>Name</th>
            <th>Display order</th>

            <!-- Add this -->
            <th></th>
        </tr>
    </thead>
    <tbody>
        @foreach (var category in Model.OrderBy(c => c.DisplayOrder))
        {
            <tr>
                <td>@category.Name</td>
                <td>@category.DisplayOrder</td>
                <!-- Add this -->
                <td>
                    <a asp-controller="Category" asp-action="Edit" asp-route-id="@category.Id" class="btn btn-dark">
                        <i class="bi bi-pencil-square"></i>
                        Edit
                    </a>
                    <a asp-controller="Category" asp-action="Delete" asp-route-id="@category.Id" class="btn btn-danger">
                        <i class="bi bi-trash-fill"></i>
                        Delete
                    </a>
                </td>
            </tr>
        }
    </tbody>
</table>
```
- Copy the Create `Get` and `Post` action methods and rename the methods to `Edit` in `CategoryController.cs`
```cs
// GET: /<controller>/Edit
public IActionResult Edit()
{
    return View();
}

// POST: /<controller>/Edit
[HttpPost]
public IActionResult Edit(Category newCategory)
{
    if(newCategory.Name == newCategory.DisplayOrder.ToString())
    {
        // "" makes this a model-only error. Instead of "", we can add a field name as well -> this makes it a field error, instead of a model-only error
        ModelState.AddModelError("", "Category Name and Display Order cannot be the same");
    }

    // ModelState.IsValid indicates if it was possible to bind the incoming values from the request to the newCategory model correctly and whether any explicitly specified validation rules for Category were broken during the model binding process.
    if (ModelState.IsValid)
    {
        _db.Categories.Add(newCategory);
        _db.SaveChanges();
        TempData["success"] = "Created a new category";
        return RedirectToAction("Index"); // (action, controller) is an overload
    }

    TempData["error"] = "Unable to create a new category";
    return View(newCategory);
}
```
- Make the following changes to the GET Action `Edit` method. Since we have appended `id` to the `asp-route` tag helper in the `Create` view Edit button link, we should name the argument as `id` here.
```cs
public IActionResult Edit(int? id)
{
    if (id == null || id == 0)
    {
        TempData["error"] = "Unable to retrieve category information as its id is invalid";
        return NotFound();
    }

    // Alternative ways to query for Category by its id
    Category? category = _db.Categories.Find(id); // works since Id is the primary key
    // Category? category = _db.Categories.FirstOrDefault(c => c.Id == id);
    // Category? category = _db.Categories.Where(c => c.Id == id).FirstOrDefault();

    if ( category == null )
    {
        TempData["error"] = "Unable to show category information as a category with the given id was not found";
        return NotFound();
    }

    return View(category);
}
```
__Note__: We can retrieve the matching category these ways as well
```cs
Category? category = _db.Categories.FirstOrDefault(c => c.Id == id); // find by non-primary key as well
```
or
```cs
Category? category = _db.Categories.Where(c => c.Id == id).FirstOrDefault(); // find by non-primary key as well
```
- Update Category
- Create a new View `Edit.cshtml` by copying `Create.cshtml` and make the necessary changes. Make sure to add `TempData` to show the message sent by the controller. Note that we have added a hidden field for passing the `Id` to the backend when the form is posted.
```cshtml
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@model Category

<div class="row mb-2">
    <h1>Edit Category</h1>
    <hr />
</div>

<partial name="_Notifications" />

<form method="post">
    <div asp-validation-summary="ModelOnly" class="text-danger"></div>

    <!-- Add this -->
    <input asp-for="Id" hidden />

    <div class="row mb-2 p-1">
        <label asp-for="Name"></label>
        <input type="text" class="form-control" asp-for="Name" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    <div class="row mb-2 p-1">
        <label asp-for="DisplayOrder"></label>
        <input type="text" class="form-control" asp-for="DisplayOrder" />
        <span asp-validation-for="DisplayOrder" class="text-danger"></span>
    </div>
    <div class="row mb-2">
        <div class="col-6 col-md-auto">
            <!-- Update this -->
            <button type="submit" class="btn btn-primary w-100">Update</button>
        </div>
        <div class="col-6 col-md-auto">
            <a asp-controller="Category" asp-action="Index" class="btn btn-secondary border w-100">Back to List</a>
        </div>
    </div>
</form>

@* This partial include is needed to enable client-side validations *@
@section Scripts {
    @{
        <partial name="_ValidationScriptsPartial" />
    }
}
```
- Also add a partial view `Views/Shared/_Notifications.cshtml`
```cshtml
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@if (TempData["success"] != null)
{
    <div class="alert alert-success">@TempData["success"]</div>
}

@if (TempData["error"] != null)
{
    <div class="alert alert-danger">@TempData["error"]</div>
}
```
- Update POST action method `Edit`
```cs
[HttpPost]
public IActionResult Edit(Category updatedCategory)
{
    if (updatedCategory.Name == updatedCategory.DisplayOrder.ToString())
    {
        // "" makes this a model-only error. Instead of "", we can add a field name as well -> this makes it a field error, instead of a model-only error
        ModelState.AddModelError("", "Category Name and Display Order cannot be the same");
    }

    if (ModelState.IsValid)
    {
        _db.Categories.Update(updatedCategory);
        _db.SaveChanges();
        TempData["success"] = "Category updated successfully";
        return RedirectToAction("Index");
    }


    TempData["error"] = "Unable to update the category. Correct the errors and try again.";
    return View(updatedCategory);
}
```
- Delete a Category
- Copy the GET and POST action methods for `Edit` action, and rename to `Delete` / `DeletePost`. Since bth action methods have same argument (`int`), we have to rename one as shown, and add the `ActionName("Delete")` attribute.
```cs
public IActionResult Delete(int? id)
{
    if (id == null || id == 0)
    {
        TempData["error"] = "Unable to retrieve category information as its id is invalid";
        return NotFound();
    }

    Category? category = _db.Categories.Find(id); // works since Id is the primary key

    if (category == null)
    {
        TempData["error"] = "Unable to show category information as a category with the given id was not found";
        return NotFound();
    }

    return View(category);
}

[HttpPost, ActionName("Delete")]
public IActionResult DeletePost(int? id)
{
    if (id == null || id == 0)
    {
        TempData["error"] = "Unable to retrieve category information as its id is invalid";
        return NotFound();
    }

    Category? category = _db.Categories.Find(id); // works since Id is the primary key

    if(category == null)
    {
        TempData["error"] = "Unable to retrieve category information as a category with the given id was not found";
        return NotFound();
    }

    _db.Categories.Remove(category);
    _db.SaveChanges();
    TempData["success"] = "Successfully deleted the category";
    return RedirectToAction("Index");
}
```
- Copy `Edit.cshtml` to create `Delete.cshtml`, and make modification as shown. Note that we disable the fields and do not need validations on delete. Hence the client-side validation script is also removed.
```cshtml
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@model Category

<div class="row mb-2">
    <h1>Delete Category</h1>
    <hr />
</div>

<partial name="_Notifications" />

<form method="post">
    <div asp-validation-summary="ModelOnly" class="text-danger"></div>

    <input asp-for="Id" hidden />

    <div class="row mb-2 p-1">
        <label asp-for="Name"></label>
        <input type="text" class="form-control" asp-for="Name" disabled />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    <div class="row mb-2 p-1">
        <label asp-for="DisplayOrder"></label>
        <input type="text" class="form-control" asp-for="DisplayOrder" disabled />
        <span asp-validation-for="DisplayOrder" class="text-danger"></span>
    </div>
    <div class="row mb-2">
        <div class="col-6 col-md-auto">
            <button type="submit" class="btn btn-danger w-100">Delete</button>
        </div>
        <div class="col-6 col-md-auto">
            <a asp-controller="Category" asp-action="Index" class="btn btn-secondary border w-100">Back to List</a>
        </div>
    </div>
</form>
```