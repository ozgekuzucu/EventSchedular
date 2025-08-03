using EventSchedular.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EventSchedular.Context
{
	public class CalendarContext : DbContext
	{
		public DbSet<Event> Events { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}