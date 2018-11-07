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
		public List<ScheduleDay> Day { get; set; }
	}
}