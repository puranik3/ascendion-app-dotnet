using System;
using Microsoft.AspNetCore.Mvc;
using AscendionAppSqlite3.Data;
using AscendionAppSqlite3.Models;

namespace AscendionAppSqlite3.Controllers;

public class CategoryController : Controller
{
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
}

