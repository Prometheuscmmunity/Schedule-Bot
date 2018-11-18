using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using TelegrammAspMvcDotNetCoreBot.Models;
using Group = TelegrammAspMvcDotNetCoreBot.Models.Group;

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{
    public class HomeWorkController
    {
        static MyContext db;
        //private static MyContext db = new MyContext(optionsBuilder.Options);

        public static void Unit()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=test36;Trusted_Connection=True;");
            db = new MyContext(optionsBuilder.Options);
        }


        public static void AddHomeWork()
        {
        }


        public static void AddHomeWorkToday(string university, string faculty, string course, string groupName,
            string text)
        {
            if (ScheduleController.IsGroupExist(university, faculty, course, groupName))
            {
                Group gr = new Group();

                DateTime now = DateTime.Now;
                string date = now.ToString("d");

                Day d = new Day();
                d.Date = date;
                d.HomeWorkText = text;

                University universitym = db.Universities.Where(m => m.Name == university).FirstOrDefault();
                Faculty facultym = db.Faculties.Where(l => l.University == universitym).Where(t => t.Name == faculty)
                    .FirstOrDefault();
                Course coursem = db.Courses.Where(o => o.Facultie == facultym).Where(t => t.Name == course)
                    .FirstOrDefault();
                gr = db.Groups.Where(g => g.Course == coursem).Where(t => t.Name == groupName).FirstOrDefault();
                d.Group = gr;

                db.Days.Add(d);
                db.SaveChanges();
            }
        }

        public static void AddHomeWorkTomorrow()
        {
        }

        public static void GetHomeWork()
        {
        }

        public static string GetHomeWorkToday(string university, string faculty, string course, string groupName)
        {
            if (ScheduleController.IsGroupExist(university, faculty, course, groupName))
            {
                DateTime now = DateTime.Now;
                string date = now.ToString("d");

                Day gr = new Day();
                gr = (from kl in db.Days
                    where kl.Date == date && kl.Group.Name == groupName && kl.Group.Course.Name == course &&
                          kl.Group.Course.Facultie.Name == faculty &&
                          kl.Group.Course.Facultie.University.Name == university
                    select kl).FirstOrDefault();


                return gr.HomeWorkText;
            }

            return "";
        }

        public static void GetHomeWorkTomorrow()
        {
        }

        public static void GetHomeWorkYesterday()
        {
        }
    }
}