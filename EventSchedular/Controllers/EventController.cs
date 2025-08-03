using EventSchedular.Context;
using EventSchedular.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventSchedular.Controllers
{
	public class EventController : Controller
	{
		private readonly CalendarContext db = new CalendarContext();
		[HttpGet]
		public ActionResult Index()
		{
			var categories = db.Categories.ToList();
			ViewBag.Categories = categories;
			return View(categories);
		}
		public ActionResult EventList()
		{
			var events = db.Events.ToList();
			return View(events);
		}

		[HttpGet]
		public JsonResult GetEvents()
		{
			var events = db.Events.ToList().Select(e => new
			{
				id = e.EventId,
				title = e.Category.CategoryName,
				start = e.StartDate.HasValue ? e.StartDate.Value.ToString("s") : null,
				end = e.EndDate.HasValue ? e.EndDate.Value.ToString("s") : null,
				backgroundColor = e.Category.CategoryColor,
				borderColor = e.Category.CategoryColor,
			}).ToList();

			return Json(events, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult CreateDraggableEvent(string title, string backgroundColor, string borderColor, int categoryId)
		{
			try
			{
				var newEvent = new Event
				{
					Title = title,
					StartDate = null,
					EndDate = null,
					CategoryId = categoryId
				};

				db.Events.Add(newEvent);
				db.SaveChanges();

				return Json(new
				{
					success = true,
					id = newEvent.EventId,
					title = newEvent.Title,
					backgroundColor,
					borderColor
				});
			}
			catch (Exception ex)
			{
				return Json(new { success = false, error = ex.Message });
			}
		}

		// Takvime sürüklendiğinde güncelle
		[HttpPost]
		public JsonResult UpdateDroppedEvent(int id, DateTime start, DateTime? end)
		{
			try
			{
				var updatedEvent = db.Events.Find(id);
				if (updatedEvent == null)
					return Json(new { success = false, message = "Event not found." });

				updatedEvent.StartDate = start;
				updatedEvent.EndDate = end ?? start;

				db.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}


		[HttpPost]
		public JsonResult CreateEvent(string Title, DateTime Start, DateTime? End, int categoryId, bool AllDay, string BackgroundColor, string BorderColor)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine($"CreateEvent called with categoryId: {categoryId}");

				if (categoryId <= 0)
				{
					return Json(new { success = false, error = "Geçersiz kategori ID" });
				}

				var newEvent = new Event
				{
					Title = Title,
					StartDate = Start,
					EndDate = End ?? Start, 
					CategoryId = categoryId
				};

				db.Events.Add(newEvent);
				db.SaveChanges();

				return Json(new { success = true, id = newEvent.EventId });
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"CreateEvent Error: {ex.Message}");
				return Json(new { success = false, error = ex.Message });
			}
		}
		[HttpPost]
		public JsonResult UpdateEvent(int id, DateTime start, DateTime? end)
		{
			try
			{
				var evt = db.Events.Find(id);
				if (evt == null)
					return Json(new { success = false, message = "Etkinlik bulunamadı. ID = " + id });

				evt.StartDate = start;
				evt.EndDate = end;
				db.SaveChanges();

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message }); 
			}
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult DeleteEvent(int id)
		{
			try
			{
				var evt = db.Events.Find(id);
				if (evt == null)
					return Json(new { success = false, message = "Etkinlik bulunamadı." });

				db.Events.Remove(evt);
				db.SaveChanges();

				return Json(new { success = true, message = "Etkinlik başarıyla silindi!" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Silme işlemi sırasında hata: " + ex.Message });
			}
		}

		[HttpGet]
		public ActionResult UpdateEventPage(int id)
		{
			var value = db.Events.Include("Category").FirstOrDefault(e => e.EventId == id);
			if (value == null)
			{
				return HttpNotFound();
			}

			var categories = db.Categories.ToList();
			ViewBag.Categories = categories;
			return View(value);
		}

		[HttpPost]
		public ActionResult UpdateEventPage(Event events)
		{
			if (!ModelState.IsValid)
			{
				var categories = db.Categories.ToList();
				ViewBag.Categories = categories;
				return View(events);
			}

			var value = db.Events.Find(events.EventId);
			if (value == null)
			{
				return Json(new { success = false, message = "Etkinlik bulunamadı!" });
			}

			try
			{
				value.Title = events.Title;
				value.StartDate = events.StartDate;
				value.EndDate = events.EndDate;
				value.CategoryId = events.CategoryId;
				db.SaveChanges();

				return RedirectToAction("EventList");
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Güncelleme sırasında hata oluştu: " + ex.Message });
			}
		}

	}
}