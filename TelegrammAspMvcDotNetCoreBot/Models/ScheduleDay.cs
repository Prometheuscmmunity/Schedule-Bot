using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegrammAspMvcDotNetCoreBot.Models
{
	public class ScheduleDay
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Day { get; set; }
		public Lesson Lesson1 { get; set; }
		public Lesson Lesson2 { get; set; }
		public Lesson Lesson3 { get; set; }
		public Lesson Lesson4 { get; set; }
		public Lesson Lesson5 { get; set; }
		public Lesson Lesson6 { get; set; }
		public Lesson Lesson7 { get; set; }
		public Lesson Lesson8 { get; set; }
	}
}
