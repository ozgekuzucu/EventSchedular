using EventSchedular.Context;
using EventSchedular.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventSchedular.Controllers
{
    public class CategoryController : Controller
    {
		private readonly CalendarContext context = new CalendarContext();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult CategoryList()
		{
			var values = context.Categories.ToList();
			return View(values);
		}
		public JsonResult GetCategories()
		{
			var categories = context.Categories.Select(c => new {
				CategoryId = c.CategoryId,
				CategoryName = c.CategoryName,
				CategoryColor = c.CategoryColor
			}).ToList();
			return Json(categories, JsonRequestBehavior.AllowGet);
		}


		[HttpGet]
		public ActionResult CreateCategory()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateCategory(Category category)
		{
			context.Categories.Add(category);
			context.SaveChanges();
			return RedirectToAction("CategoryList");
		}

		public ActionResult DeleteCategory(int id)
		{
			var value = context.Categories.Find(id);
			context.Categories.Remove(value);
			context.SaveChanges();
			return RedirectToAction("CategoryList");
		}

		[HttpGet]
		public ActionResult UpdateCategory(int id)
		{
			var value = context.Categories.Find(id);
			return View(value);
		}

		[HttpPost]
		public ActionResult UpdateCategory(Category category)
		{
			var value = context.Categories.Find(category.CategoryId);
			value.CategoryName = category.CategoryName;
			value.CategoryColor = category.CategoryColor;
			context.SaveChanges();
			return RedirectToAction("CategoryList");
		}
	}
}