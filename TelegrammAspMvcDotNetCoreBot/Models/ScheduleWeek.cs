using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegrammAspMvcDotNetCoreBot.Models
{
	public class ScheduleWeek
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Week { get; set; }
		public Group Group { get; set; }
		public ScheduleDay Day1 { get; set; }
		public ScheduleDay Day2 { get; set; }
		public ScheduleDay Day3 { get; set; }
		public ScheduleDay Day4 { get; set; }
		public ScheduleDay Day5 { get; set; }
		public ScheduleDay Day6 { get; set; }
		public ScheduleDay Day7 { get; set; }
	}
}