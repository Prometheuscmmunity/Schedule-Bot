using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using TelegrammAspMvcDotNetCoreBot.Models.ScheduleExceptions.AlreadyExists;
using TelegrammAspMvcDotNetCoreBot.Models.ScheduleExceptions.DoesntExists;

namespace ScheduleController
{
    class Program
    {
        private static XDocument xDoc = XDocument.Load("un.xml");

        private static XElement xRoot = xDoc.Root;

        private static readonly string[] weekDays =
            {"monday", "tuesday", "wednesday", "thursday", "friday", "saturday"};


        private static void XRootReload(ref XElement XRoot)
        {
            XDocument xDoc = XDocument.Load("un.xml");
            XRoot = xDoc.Root;
        }

        private static bool IsUniverExist(string name)
        {
            return xRoot.Elements("university").Any(s => s.Attribute("name").Value.ToLower() == name.ToLower());
        }

        private static bool IsFacExist(string univerName, string facName)
        {
            if (IsUniverExist(univerName))
            {
                var items =
                    from xe in xRoot.Elements("university")
                    where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                    select xe;

                bool exist = items.Elements("faculty")
                    .Any(s => s.Attribute("name").Value.ToLower() == facName.ToLower());
                return exist;
            }
            else
            {
                return false;
            }
        }

        private static bool IsCourseExist(string univerName, string facName, int num)
        {
            if (IsUniverExist(univerName) && IsFacExist(univerName, facName))
            {
                var items =
                    from xe in xRoot.Elements("university")
                    where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                    select xe;
                var facs =
                    from f in items.Elements("faculty")
                    where f.Attribute("name").Value.ToLower() == facName.ToLower()
                    select f;

                bool exist = facs.Elements("course")
                    .Any(c => c.Attribute("num").Value == num.ToString());
                return exist;
            }
            else
            {
                return false;
            }
        }

        private static bool IsGroupExist(string univerName, string facName, int num, string groupId)
        {
            if (IsUniverExist(univerName) && IsFacExist(univerName, facName) && IsCourseExist(univerName, facName, num))
            {
                var items =
                    from xe in xRoot.Elements("university")
                    where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                    select xe;
                var facs =
                    from f in items.Elements("faculty")
                    where f.Attribute("name").Value.ToLower() == facName.ToLower()
                    select f;

                var courses =
                    from c in facs.Elements("course")
                    where c.Attribute("num").Value == num.ToString()
                    select c;

                bool exist = courses.Elements("group")
                    .Any(g => g.Attribute("id").Value == groupId);
                return exist;
            }
            else
            {
                return false;
            }
        }

        private static bool IsWeekExist(string univerName, string facName, int num, string groupId, int week)
        {
            if (IsUniverExist(univerName) && IsFacExist(univerName, facName) &&
                IsCourseExist(univerName, facName, num) && IsGroupExist(univerName, facName, num, groupId))
            {
                var items =
                    from xe in xRoot.Elements("university")
                    where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                    select xe;
                var facs =
                    from f in items.Elements("faculty")
                    where f.Attribute("name").Value.ToLower() == facName.ToLower()
                    select f;

                var courses =
                    from c in facs.Elements("course")
                    where c.Attribute("num").Value == num.ToString()
                    select c;

                var group =
                    from g in courses.Elements("group")
                    where g.Attribute("id").Value.ToLower() == groupId.ToLower()
                    select g;
                string weekName;
                if (week == 1)
                {
                    weekName = "even-week";
                }
                else if (week == 2)

                {
                    weekName = "odd-week";
                }
                else
                {
                    return false;
                }

                bool exist = group.Elements()
                    .Any(w => w.Name.LocalName.ToLower() == weekName.ToLower());
                return exist;
            }

            return false;
        }

        private static List<string> GetUniversities()
        {
            var items =
                from xe in xRoot.Elements("university")
                select xe.Attribute("name").Value;
            List<string> univerNames = items.ToList();
            return univerNames;
        }

        private static List<string> GetFaculties(string univerName)
        {
            if (IsUniverExist(univerName))
            {
                var items =
                    from xe in xRoot.Elements("university")
                    where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                    select xe;

                var facsTemp =
                    from f in items.Elements("faculty")
                    select f.Attribute("name").Value;

                List<string> facs = facsTemp.ToList();

                return facs;
            }
            else
            {
                throw new UniversityDoesntExistsException();
            }
        }

        private static List<string> GetCourses(string univerName, string facName)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    var items =
                        from xe in xRoot.Elements("university")
                        where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                        select xe;

                    var facsTemp =
                        from f in items.Elements("faculty")
                        where f.Attribute("name").Value.ToLower() == facName.ToLower()
                        select f;

                    var courseListTemp =
                        from c in facsTemp.Elements("course")
                        select c.Attribute("num").Value;
                    List<string> courseList = courseListTemp.ToList();
                    return courseList;
                }

                throw new FacultyDoesntExistsException();
            }

            throw new UniversityDoesntExistsException();
        }

        private static List<string> GetGroups(string univerName, string facName, int course)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    if (IsCourseExist(univerName, facName, course))
                    {
                        var items =
                            from xe in xRoot.Elements("university")
                            where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                            select xe;

                        var facsTemp =
                            from f in items.Elements("faculty")
                            where f.Attribute("name").Value.ToLower() == facName.ToLower()
                            select f;

                        var courseListTemp =
                            from c in facsTemp.Elements("course")
                            where c.Attribute("num").Value.ToLower() == course.ToString()
                            select c;

                        var groupsListTemp =
                            from g in courseListTemp.Elements("group")
                            select g.Attribute("id").Value;

                        List<string> groupList = groupsListTemp.ToList();
                        return groupList;
                    }

                    throw new CourseDoesntExistsException();
                }

                throw new FacultyDoesntExistsException();
            }

            throw new UniversityDoesntExistsException();
        }

        private static List<Para> GetDaysShedule(string univerName, string facName, int course, string groupId,
            int week, int weekDay)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    if (IsCourseExist(univerName, facName, course))
                    {
                        if (IsGroupExist(univerName, facName, course, groupId))
                        {
                            var items =
                                from xe in xRoot.Elements("university")
                                where xe.Attribute("name").Value.ToLower() == univerName.ToLower()
                                select xe;

                            var facsTemp =
                                from f in items.Elements("faculty")
                                where f.Attribute("name").Value.ToLower() == facName.ToLower()
                                select f;

                            var courseListTemp =
                                from c in facsTemp.Elements("course")
                                where c.Attribute("num").Value.ToLower() == course.ToString()
                                select c;

                            var groupsListTemp =
                                from g in courseListTemp.Elements("group")
                                where g.Attribute("id").Value.ToLower() == groupId.ToLower()
                                select g;


                            List<Para> schedule = new List<Para>();
                            if (week == 1)
                            {
                                XElement xWeek = groupsListTemp.ToArray().First().Element("even-week");
                                XElement day = xWeek.Elements().ToList()[weekDay - 1];

                                foreach (var p in day.Elements())
                                {
                                    Para temp = new Para(
                                        p.Value,
                                        p.Attribute("time").Value,
                                        p.Attribute("room").Value,
                                        p.Attribute("prepod").Value);
                                    schedule.Add(temp);
                                }

                                return schedule;
                            }

                            if (week == 2)
                            {
                                XElement xWeek = groupsListTemp.ToArray().First().Element("odd-week");
                                XElement day = xWeek.Elements().ToList()[weekDay - 1];

                                foreach (var p in day.Elements())
                                {
                                    Para temp = new Para(
                                        p.Value,
                                        p.Attribute("time").Value,
                                        p.Attribute("room").Value,
                                        p.Attribute("prepod").Value);
                                    schedule.Add(temp);
                                }

                                return schedule;
                            }

                            throw new WeekDoesntExistsException();
                        }

                        throw new GroupDoesntExistsException();
                    }

                    throw new CourseDoesntExistsException();
                }

                throw new FacultyDoesntExistsException();
            }

            throw new UniversityDoesntExistsException();
        }

        private static void AddUniversity(string name)
        {
            if (!IsUniverExist(name))
            {
                XElement univer = new XElement("university");
                XAttribute univerNameAttr = new XAttribute("name", name);
                univer.Add(univerNameAttr);
                xRoot.Add(univer);
                xRoot.Save("un.xml");
                XRootReload(ref xRoot);
            }
            else
            {
                throw new UniversityAlreadyExistsException();
            }
        }

        private static void AddFaculty(string univerName, string facName)
        {
            if (IsUniverExist(univerName))
            {
                if (!IsFacExist(univerName, facName))
                {
                    var items =
                        from un in xRoot.Elements("university")
                        where un.Attribute("name").Value.ToLower().Equals(univerName.ToLower())
                        select un;
                    XElement univer = items.ToList().First();
                    XElement faculty = new XElement("faculty");
                    XAttribute facultyName = new XAttribute("name", facName);
                    faculty.Add(facultyName);
                    univer.Add(faculty);
                    xRoot.Save("un.xml");
                    XRootReload(ref xRoot);
                }
                else
                {
                    throw new FacultyAlreadyExistsException();
                }
            }
            else
            {
                throw new UniversityDoesntExistsException();
            }
        }

        private static void AddCourse(string univerName, string facName, int num)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    if (!IsCourseExist(univerName, facName, num))
                    {
                        XElement univer =
                            (from un in xRoot.Elements("university")
                                where un.Attribute("name").Value.ToLower().Equals(univerName.ToLower())
                                select un).ToList().First();
                        XElement fac =
                            (from f in univer.Elements("faculty")
                                where f.Attribute("name").Value.ToLower().Equals(facName.ToLower())
                                select f).ToList().First();

                        XElement course = new XElement("course");
                        XAttribute courseName = new XAttribute("num", num.ToString());
                        course.Add(courseName);
                        fac.Add(course);
                        xRoot.Save("un.xml");
                        XRootReload(ref xRoot);
                    }
                    else
                    {
                        throw new CourseAlreadyExistsException();
                    }
                }
                else
                {
                    throw new FacultyDoesntExistsException();
                }
            }
            else
            {
                throw new UniversityDoesntExistsException();
            }
        }

        private static void AddGroup(string univerName, string facName, int num, string groupId)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    if (IsCourseExist(univerName, facName, num))
                    {
                        if (!IsGroupExist(univerName, facName, num, groupId))
                        {
                            XElement univer =
                                (from un in xRoot.Elements("university")
                                    where un.Attribute("name").Value.ToLower().Equals(univerName.ToLower())
                                    select un).ToList().First();
                            XElement fac =
                                (from f in univer.Elements("faculty")
                                    where f.Attribute("name").Value.ToLower().Equals(facName.ToLower())
                                    select f).ToList().First();

                            XElement course =
                                (from c in fac.Elements("course")
                                    where c.Attribute("num").Value.ToLower().Equals(num.ToString())
                                    select c).ToList().First();
                            XElement group = new XElement("group");
                            XAttribute groupName = new XAttribute("id", groupId);
                            group.Add(groupName);
                            course.Add(group);
                            xRoot.Save("un.xml");
                            XRootReload(ref xRoot);
                        }
                        else
                        {
                            throw new GroupAlreadyExistsException();
                        }
                    }
                    else
                    {
                        throw new CourseDoesntExistsException();
                    }
                }
                else
                {
                    throw new FacultyDoesntExistsException();
                }
            }
            else
            {
                throw new UniversityDoesntExistsException();
            }
        }

        private static void AddWeek(string univerName, string facName, int num, string groupId, int week)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    if (IsCourseExist(univerName, facName, num))
                    {
                        if (IsGroupExist(univerName, facName, num, groupId))
                        {
                            XElement univer =
                                (from un in xRoot.Elements("university")
                                    where un.Attribute("name").Value.ToLower().Equals(univerName.ToLower())
                                    select un).ToList().First();
                            XElement fac =
                                (from f in univer.Elements("faculty")
                                    where f.Attribute("name").Value.ToLower().Equals(facName.ToLower())
                                    select f).ToList().First();

                            XElement course =
                                (from c in fac.Elements("course")
                                    where c.Attribute("num").Value.ToLower().Equals(num.ToString())
                                    select c).ToList().First();
                            XElement group =
                                (from g in course.Elements("group")
                                    where g.Attribute("id").Value.ToLower().Equals(groupId.ToLower())
                                    select g).ToList().First();
                            XElement curWeek;
                            if (week == 1)
                            {
                                curWeek = new XElement("even-week");
                            }
                            else if (week == 2)
                            {
                                curWeek = new XElement("odd-week");
                            }
                            else
                            {
                                throw new WeekDoesntExistsException();
                            }

                            group.Add(curWeek);
                            xRoot.Save("un.xml");
                            XRootReload(ref xRoot);
                        }
                        else
                        {
                            throw new GroupDoesntExistsException();
                        }
                    }
                    else
                    {
                        throw new CourseDoesntExistsException();
                    }
                }
                else
                {
                    throw new FacultyDoesntExistsException();
                }
            }
            else
            {
                throw new UniversityDoesntExistsException();
            }
        }

        private static void AddSheduleDay(string univerName, string facName, int num, string groupId, int week, int day,
            List<Para> shed)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    if (IsCourseExist(univerName, facName, num))
                    {
                        if (IsGroupExist(univerName, facName, num, groupId))
                        {
                            if (IsWeekExist(univerName, facName, num, groupId, week))
                            {
                                XElement univer =
                                    (from un in xRoot.Elements("university")
                                        where un.Attribute("name").Value.ToLower().Equals(univerName.ToLower())
                                        select un).ToList().First();
                                XElement fac =
                                    (from f in univer.Elements("faculty")
                                        where f.Attribute("name").Value.ToLower().Equals(facName.ToLower())
                                        select f).ToList().First();

                                XElement course =
                                    (from c in fac.Elements("course")
                                        where c.Attribute("num").Value.ToLower().Equals(num.ToString())
                                        select c).ToList().First();
                                XElement group =
                                    (from g in course.Elements("group")
                                        where g.Attribute("id").Value.ToLower().Equals(groupId.ToLower())
                                        select g).ToList().First();
                                XElement curWeek;
                                if (week == 1)
                                {
                                    curWeek = group.Element("even-week");
                                }
                                else if (week == 2)
                                {
                                    curWeek = group.Element("odd-week");
                                }
                                else
                                {
                                    throw new WeekDoesntExistsException();
                                }

                                group.Add(curWeek);
                                XElement curDay = new XElement(weekDays[day - 1]);
                                curWeek.Add(curDay);

                                for (int i = 0; i < shed.Count; i++)
                                {
                                    XElement para = new XElement($"para{i + 1}");
                                    para.Value = shed[i].name;
                                    XAttribute time = new XAttribute("time", shed[i].time);
                                    XAttribute room = new XAttribute("room", shed[i].room);
                                    XAttribute prepod = new XAttribute("prepod", shed[i].prepod);
                                    para.Add(time);
                                    para.Add(room);
                                    para.Add(prepod);
                                    curDay.Add(para);
                                }

                                xRoot.Save("un.xml");
                                XRootReload(ref xRoot);
                            }
                            else
                            {
                                throw new WeekDoesntExistsException();
                            }
                        }
                        else
                        {
                            throw new GroupDoesntExistsException();
                        }
                    }
                    else
                    {
                        throw new CourseDoesntExistsException();
                    }
                }
                else
                {
                    throw new FacultyDoesntExistsException();
                }
            }
            else
            {
                throw new UniversityDoesntExistsException();
            }
        }
        private static void AddSheduleWeek(string univerName, string facName, int num, string groupId, int week, List<List<Para>> shed)
        {
            if (IsUniverExist(univerName))
            {
                if (IsFacExist(univerName, facName))
                {
                    if (IsCourseExist(univerName, facName, num))
                    {
                        if (IsGroupExist(univerName, facName, num, groupId))
                        {
                            if (IsWeekExist(univerName, facName, num, groupId, week))
                            {
                                XElement univer =
                                    (from un in xRoot.Elements("university")
                                        where un.Attribute("name").Value.ToLower().Equals(univerName.ToLower())
                                        select un).ToList().First();
                                XElement fac =
                                    (from f in univer.Elements("faculty")
                                        where f.Attribute("name").Value.ToLower().Equals(facName.ToLower())
                                        select f).ToList().First();

                                XElement course =
                                    (from c in fac.Elements("course")
                                        where c.Attribute("num").Value.ToLower().Equals(num.ToString())
                                        select c).ToList().First();
                                XElement group =
                                    (from g in course.Elements("group")
                                        where g.Attribute("id").Value.ToLower().Equals(groupId.ToLower())
                                        select g).ToList().First();
                                XElement curWeek;
                                if (week == 1)
                                {
                                    curWeek = group.Element("even-week");
                                }
                                else if (week == 2)
                                {
                                    curWeek = group.Element("odd-week");
                                }
                                else
                                {
                                    throw new WeekDoesntExistsException();
                                }

                                group.Add(curWeek);
                                for (int i = 0; i < shed.Count; i++)
                                {
                                    XElement curDay = new XElement(weekDays[i]);
                                    curWeek.Add(curDay);
                                    for (int j = 0; j < shed[i].Count; j++)
                                    {
                                        XElement para = new XElement($"para{j + 1}");
                                        para.Value = shed[i][j].name;
                                        XAttribute time = new XAttribute("time", shed[i][j].time);
                                        XAttribute room = new XAttribute("room", shed[i][j].room);
                                        XAttribute prepod = new XAttribute("prepod", shed[i][j].prepod);
                                        para.Add(time);
                                        para.Add(room);
                                        para.Add(prepod);
                                        curDay.Add(para);
                                    }
                                }          

                                xRoot.Save("un.xml");
                                XRootReload(ref xRoot);
                            }
                            else
                            {
                                throw new WeekDoesntExistsException();
                            }
                        }
                        else
                        {
                            throw new GroupDoesntExistsException();
                        }
                    }
                    else
                    {
                        throw new CourseDoesntExistsException();
                    }
                }
                else
                {
                    throw new FacultyDoesntExistsException();
                }
            }
            else
            {
                throw new UniversityDoesntExistsException();
            }
        }
    }
}