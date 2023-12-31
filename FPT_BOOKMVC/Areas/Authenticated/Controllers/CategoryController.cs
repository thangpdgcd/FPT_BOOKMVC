﻿using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Models;
using FPT_BOOKMVC.ModelsCRUD.Category;
using FPT_BOOKMVC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
    //area
    [Area(SD.AuthenticatedArea)]
    [Authorize(Roles = SD.StoreOwnerRole + "," + SD.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context; 

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
		public async Task<IActionResult> CategoryIndex()
        { 
          List <Category> category = await context.Categories.Where(x => x.IsApproved).ToListAsync();
			return View(category);
        }
       
        [HttpGet] 
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost] 

        public async Task<IActionResult> CreateCategory(AddCategoryViewModel CategoryModel)
        {
            var category = new Category() 
            {
                Name = CategoryModel.Name,
				Description = CategoryModel.Description
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return RedirectToAction("CategoryIndex");
        }
        [HttpGet]
		public async Task<IActionResult> ViewCategory(int id)
		{
            var category = await context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id); 
			if (category != null)
            {
                var viewmodel = new UpdateCategoryView()
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    Description = category.Description
                };
                //create task new and run on a threadpool ,not create thread same
                return View("ViewCategory", viewmodel);
			}
            return RedirectToAction("CategoryIndex");
        }
        [HttpPost]
        public async Task<IActionResult> ViewCategory(UpdateCategoryView model)
        {
            var category = await context.Categories.FindAsync(model.CategoryId);
			if (category != null)
            {
                category.Name = model.Name;
                category.Description = model.Description;

                await context.SaveChangesAsync();

                return RedirectToAction("CategoryIndex");
            }

            return RedirectToAction("CategoryIndex");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(UpdateCategoryView model)
        {
            var category = await context.Categories.FindAsync(model.CategoryId);

            if (category != null)
            {
                context.Categories.Remove(category);

                await context.SaveChangesAsync();

                return RedirectToAction("CategoryIndex");
            }

            return RedirectToAction("CategoryIndex");
        }
		[HttpGet]
		public async Task<IActionResult> CategoryApproved()
		{
            //list was approve
			var category = await context.Categories.ToListAsync();
			return View(category);
		}
        //identify URL to handle, id is a parameter
		[HttpPost("{id}/approve")] 
        public async Task<IActionResult> Approve(int id)
        {
            var category = await context.Categories.FindAsync(id);
			if (category == null)
            {
                return NotFound();
            }
            category.IsApproved = true;
			context.SaveChanges();
            return RedirectToAction("CategoryApproved");
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            context.Categories.Remove(category);
            context.SaveChanges();

            // code to notify store owner of category rejection here
            return RedirectToAction("CategoryApproved");
        }
    }
}
