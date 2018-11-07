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
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=mobilesdb2;Trusted_Connection=True;");
			db = new MyContext(optionsBuilder.Options);
		}

		public static void AddUniversity(string name)
		{

			//MyContext dbContext = new MyContext(optionsBuilder.Options);
			//var controller = new ScheduleController(dbContext);

			if (!IsUniversityExist(name))
			{
				University un = new University();
				un.Name = name;

				db.Universities.Add(un);
				db.SaveChanges();
			}
		}

		public static void AddFaculty(string university, string name)
		{
			if (!IsFacultyExist(university, name))
			{
				Faculty fac = new Faculty();
				fac.Name = name;
				fac.University = db.Universities.Where(n => n.Name == university).FirstOrDefault();

				db.Faculties.Add(fac);
				db.SaveChanges();
			}
		}

		public static void AddCourse(string university, string faculty, string name)
		{
			if (!IsCourseExist(university, faculty, name))
			{
				Course co = new Course();
				co.Name = name;
				co.Facultie = db.Faculties.Where(n => n.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(x => x.Name == faculty).FirstOrDefault();

				db.Courses.Add(co);
				db.SaveChanges();
			}
		}

		public static void AddGroup(string university, string faculty, string course, string name)
		{
			if (!IsGroupExist(university, faculty, course, name))
			{
				Group gr = new Group();
				gr.Name = name;
				gr.Course = db.Courses.Where(l => l.Facultie == db.Faculties.Where(n => n.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(x => x.Name == faculty).FirstOrDefault()).Where(x => x.Name == course).FirstOrDefault();

				db.Groups.Add(gr);
				db.SaveChanges();
			}
		}



		public static void AddScheduleWeek(string university, string faculty, string course, string group, ScheduleWeek week)
		{
			week.Group = db.Groups.Where(c => c.Course == db.Courses.Where(ll => ll.Facultie == db.Faculties.Where(n => n.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(x => x.Name == faculty).FirstOrDefault()).Where(x => x.Name == course).FirstOrDefault()).Where(v => v.Name == group).FirstOrDefault();
			db.ScheduleWeeks.Add(week);
			db.SaveChanges();
		}
		


		public static bool IsUniversityExist(string university)
		{
			bool result = false;

			if (db.Universities.Where(n => n.Name == university).FirstOrDefault() != null)
				result = true;

			return result;
		}

		public static bool IsFacultyExist(string university, string faculty)
		{
			bool result = false;

			if (db.Faculties.Where(m => m.University == db.Universities.Where(n => n.Name == university).FirstOrDefault()).Where(e => e.Name == faculty).FirstOrDefault() != null)
				result = true;

			return result;
		}

		public static bool IsCourseExist(string university, string faculty, string course)
		{
			bool result = false;

			if (db.Courses.Where(l => l.Facultie == db.Faculties.Where(m => m.University == db.Universities.Where(n => n.Name == university).FirstOrDefault()).FirstOrDefault()).Where(e => e.Name == course).FirstOrDefault() != null)
				result = true;

			return result;
		}

		public static bool IsGroupExist(string university, string faculty, string course, string group)
		{
			bool result = false;

			if (db.Groups.Where(o => o.Course == db.Courses.Where(l => l.Facultie == db.Faculties.Where(m => m.University == db.Universities.Where(n => n.Name == university).FirstOrDefault()).FirstOrDefault()).FirstOrDefault()).Where(e => e.Name == group).FirstOrDefault() != null)
				result = true;
			
			return result;
		}



		public static List<string> GetUniversities()
		{
			List<string> result = new List<string>();
			List<University> source = db.Universities.ToList();

			foreach (University item in source)
			{
				result.Add(item.Name);
			}

			return result;
		}

		public static List<string> GetFaculties(string university)
		{
			List<string> result = new List<string>();

			University universitym = db.Universities.Where(m => m.Name == university).FirstOrDefault();
			
			List<Faculty> source = db.Faculties.Where(n => n.University == universitym).ToList();

			foreach (Faculty item in source)
			{
				result.Add(item.Name);
			}

			return result;
		}

		public static List<string> GetCourses(string university, string faculty)
		{
			University universitym = db.Universities.Where(m => m.Name == university).FirstOrDefault();
			Faculty facultym = db.Faculties.Where(l => l.University == universitym).Where(t => t.Name == faculty).FirstOrDefault();
			
			List<string> result = new List<string>();
			List<Course> source = db.Courses.Where(n => n.Facultie == facultym).ToList();

			foreach (Course item in source)
			{
				result.Add(item.Name);
			}

			return result;
		}

		public static List<string> GetGroups(string university, string faculty, string course)
		{
			University universitym = db.Universities.Where(m => m.Name == university).FirstOrDefault();
			Faculty facultym = db.Faculties.Where(l => l.University == universitym).Where(t => t.Name == faculty).FirstOrDefault();
			Course coursem = db.Courses.Where(o => o.Facultie == facultym).Where(t => t.Name == course).FirstOrDefault();

			List<string> result = new List<string>();
			List<Group> source = db.Groups.Where(n => n.Course == coursem).ToList();

			foreach (Group item in source)
			{
				result.Add(item.Name);
			}

			return result;
		}

		public static ScheduleDay GetSchedule(string university, string faculty, string course, string group, int week, int day)
		{
			University universitym = db.Universities.Where(m => m.Name == university).FirstOrDefault();

			Faculty facultym = db.Faculties.Where(l => l.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).ToList().Where(t => t.Name == faculty).FirstOrDefault();

			Course coursem = db.Courses
				.Where(o => o.Facultie == db.Faculties
					.Where(l => l.University == db.Universities
						.Where(m => m.Name == university).FirstOrDefault())
							.Where(t => t.Name == faculty).FirstOrDefault())
								.Where(t => t.Name == course).FirstOrDefault();

			Group groupm = db.Groups.Where(n => n.Course == db.Courses.Where(o => o.Facultie == db.Faculties.Where(l => l.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(t => t.Name == faculty).FirstOrDefault()).Where(t => t.Name == course).FirstOrDefault()).ToList().Where(t => t.Name == group).FirstOrDefault();

			List<ScheduleDay> li = db.ScheduleWeeks.Where(n => n.Group == db.Groups.Where(k => k.Course == db.Courses.Where(o => o.Facultie == db.Faculties.Where(l => l.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(t => t.Name == faculty).FirstOrDefault()).Where(t => t.Name == course).FirstOrDefault()).Where(t => t.Name == group).FirstOrDefault()).Where(m => m.Week == week).FirstOrDefault().Day;

			foreach (ScheduleDay item in db.ScheduleWeeks.Where(n => n.Group == db.Groups.Where(k => k.Course == db.Courses.Where(o => o.Facultie == db.Faculties.Where(l => l.University == db.Universities.Where(m => m.Name == university).FirstOrDefault()).Where(t => t.Name == faculty).FirstOrDefault()).Where(t => t.Name == course).FirstOrDefault()).Where(t => t.Name == group).FirstOrDefault()).Where(m => m.Week == week).FirstOrDefault().Day)
			{
				if (item.Day == day)
					return item;
			}

			return null;
		}
	}
}