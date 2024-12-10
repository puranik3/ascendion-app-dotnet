using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AscendionApp.Data;
using AscendionApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AscendionApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();
            return View(categories);
        }

        // GET: /<controller>/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /<controller>/Create
        [HttpPost]
        public IActionResult Create(Category newCategory)
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
    }
}

