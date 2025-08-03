﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventSchedular.Entities
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string CategoryColor { get; set; }
		public List<Event> Events { get; set; }
	}
}