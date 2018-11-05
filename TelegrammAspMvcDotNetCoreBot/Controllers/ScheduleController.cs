using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using TelegrammAspMvcDotNetCoreBot.Models.ScheduleExceptions.DoesntExists;
using TelegrammAspMvcDotNetCoreBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{
	public static class ScheduleController
	{

		//static DbContextOptionsBuilder<MyContext> optionsBuilder = new DbContextOptionsBuilder<MyContext>().UseSqlite("Server=(localdb)\\mssqllocaldb;Database=mobilesdb;Trusted_Connection=True;");

		//public MyContext db = HttpContext.RequestServices.GetService<MyContext>();

		static MyContext db;
		//private static MyContext db = new MyContext(optionsBuilder.Options);
		
		public static void Unit()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
			optionsBuilder.UseSqlServer("Server=localhost;Database=u0473827_bot;User Id=u0473827_bot;Password=a12345!;");
			db = new MyContext(optionsBuilder.Options);
		}

		public static void AddUniversity(string name)
		{

			//MyContext dbContext = new MyContext(optionsBuilder.Options);
			//var controller = new ScheduleController(dbContext);

			University un = new University();
			un.Name = name;

			db.Universities.Add(un);
			db.SaveChanges();
		}

		public static void AddFaculty(string university, string name)
		{
			Faculty fac = new Faculty();
			fac.Name = name;
			fac.University = db.Universities.Where(n => n.Name == university).FirstOrDefault();

			db.Faculties.Add(fac);
			db.SaveChanges();
		}

		public static void AddCourse(string university, string faculty, string name)
		{
			Course co = new Course();
			co.Name = name;
			co.Facultie = db.Faculties.Where(n => n.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(x => x.Name == faculty).FirstOrDefault();

			db.Courses.Add(co);
			db.SaveChanges();
		}

		public static void AddGroup(string university, string faculty, string course, string name)
		{
			Group gr = new Group();
			gr.Name = name;
			gr.Course = db.Courses.Where(l => l.Facultie == db.Faculties.Where(n => n.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(x => x.Name == faculty).FirstOrDefault()).Where(x => x.Name == course).FirstOrDefault();

			db.Groups.Add(gr);
			db.SaveChanges();
		}

		public static void AddScheduleWeek(string university, string faculty, string course, string group, string name, ScheduleWeek week)
		{
			week.Group = db.Groups.Where(c => c.Course == db.Courses.Where(ll => ll.Facultie == db.Faculties.Where(n => n.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(x => x.Name == faculty).FirstOrDefault()).Where(x => x.Name == course).FirstOrDefault()).FirstOrDefault();
			db.ScheduleWeeks.Add(week);
			db.SaveChanges();
		}


		private static readonly string[] weekDays =
            {"monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"};


	}
}