

using System;

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{
    public class HomeWorkController
    {


        public static void AddHomeWork()
        {
            
        }


        public static string AddHomeWorkToday()
        {
            DateTime now = DateTime.Now;
            string today = now.ToString("dd.MM.yyyy");
            return today;
        }

        public static void AddHomeWorkTomorrow()
        {
            
        }

        public static void GetHomeWork()
        {
            
        }

        public static void GetHomeWorkToday()
        {
         
        }

        public static void GetHomeWorkTomorrow()
        {
            
        }

        public static void GetHomeWorkYesterday()
        {
            
        }


    }
}