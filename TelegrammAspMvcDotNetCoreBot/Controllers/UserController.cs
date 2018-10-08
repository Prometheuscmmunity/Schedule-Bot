using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{
    public class UserController
	{
		public static bool CheckUserAv(long UserId)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;

			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			if (childnode != null)
				return true;

			return false;
		}

		public static void CheckUser(long UserId)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;

			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			if (childnode == null)
				CreateUser(UserId);
			else
				ReCreateUser(UserId);
		}

		public static void ReCreateUser(long UserId)
		{

			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			xRoot.RemoveChild(childnode);
			xDoc.Save("users.xml");

			CreateUser(UserId);
		}

		public static void CreateUser(long UserId)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			// создаем новый элемент user 
			XmlElement userElem = xDoc.CreateElement("user");
			// создаем атрибут name 
			XmlAttribute idAttr = xDoc.CreateAttribute("id");
			// создаем элементы university и faculty 
			XmlElement universityElem = xDoc.CreateElement("university");
			XmlElement facultyElem = xDoc.CreateElement("faculty");
			XmlElement courseElem = xDoc.CreateElement("course");
			XmlElement groupElem = xDoc.CreateElement("group");
			// создаем текстовые значения для элементов и атрибута 
			XmlText idText = xDoc.CreateTextNode(UserId.ToString());
			XmlText universityText = xDoc.CreateTextNode("");
			XmlText facultyText = xDoc.CreateTextNode("");
			XmlText courseText = xDoc.CreateTextNode("");
			XmlText groupText = xDoc.CreateTextNode("");

			//добавляем узлы 
			idAttr.AppendChild(idText);
			universityElem.AppendChild(universityText);
			facultyElem.AppendChild(facultyText);
			courseElem.AppendChild(courseText);
			groupElem.AppendChild(groupText);

			userElem.Attributes.Append(idAttr);
			userElem.AppendChild(universityElem);
			userElem.AppendChild(facultyElem);
			userElem.AppendChild(courseElem);
			userElem.AppendChild(groupElem);

			xRoot.AppendChild(userElem);
			xDoc.Save("users.xml");
		}

		public static void EditUser(long UserId, string param)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			foreach (XmlNode child in childnode.ChildNodes)
			{
				if (child.InnerText == "")
				{
					EditUserInf(UserId, param, child.Name);
					break;

					if (child.Name == "university")
					{
						EditUserUniversity(UserId, param);
						break;
					}
					if (child.Name == "faculty")
					{
						EditUserFaculty(UserId, param);
						break;
					}
					if (child.Name == "course")
					{
						EditUserCourse(UserId, param);
						break;
					}
					if (child.Name == "group")
					{
						EditUserGroup(UserId, param);
						break;
					}
				}
			}
		}

		private static void EditUserInf(long UserId, string param, string type)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			foreach (XmlNode child in childnode.ChildNodes)
			{
				if (child.Name == type)
				{
					childnode.RemoveChild(child);
					XmlElement typeElem = xDoc.CreateElement("type");
					XmlText typeText = xDoc.CreateTextNode(param);
					typeElem.AppendChild(typeText);
					childnode.AppendChild(typeElem);
					xDoc.Save("users.xml");
					break;
				}
			}
		}

		public static void EditUserUniversity(long UserId, string param)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			foreach (XmlNode child in childnode.ChildNodes)
			{
				if (child.Name == "university")
				{
					childnode.RemoveChild(child);
					XmlElement universityElem = xDoc.CreateElement("university");
					XmlText universityText = xDoc.CreateTextNode(param);
					universityElem.AppendChild(universityText);
					childnode.AppendChild(universityElem);
					xDoc.Save("users.xml");
					break;
				}
			}
		}

		public static void EditUserFaculty(long UserId, string param)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			foreach (XmlNode child in childnode.ChildNodes)
			{
				if (child.Name == "faculty")
				{
					childnode.RemoveChild(child);
					XmlElement facultyElem = xDoc.CreateElement("faculty");
					XmlText facultyText = xDoc.CreateTextNode(param);
					facultyElem.AppendChild(facultyText);
					childnode.AppendChild(facultyElem);
					xDoc.Save("users.xml");
					break;
				}
			}
		}

		public static void EditUserCourse(long UserId, string param)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			foreach (XmlNode child in childnode.ChildNodes)
			{
				if (child.Name == "course")
				{
					childnode.RemoveChild(child);
					XmlElement courseElem = xDoc.CreateElement("course");
					XmlText courseText = xDoc.CreateTextNode(param);
					courseElem.AppendChild(courseText);
					childnode.AppendChild(courseElem);
					xDoc.Save("users.xml");
					break;
				}
			}
		}

		public static void EditUserGroup(long UserId, string param)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

			foreach (XmlNode child in childnode.ChildNodes)
			{
				if (child.Name == "group")
				{
					childnode.RemoveChild(child);
					XmlElement groupElem = xDoc.CreateElement("group");
					XmlText groupText = xDoc.CreateTextNode(param);
					groupElem.AppendChild(groupText);
					childnode.AppendChild(groupElem);
					xDoc.Save("users.xml");
					break;
				}
			}
		}

		public static string Get(long UserId, string Type)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlNode childnode = xRoot.SelectSingleNode("user[@id='" + UserId + "']");


			foreach (XmlNode child in childnode.ChildNodes)
			{
				if (child.Name == Type)
				{
					return child.InnerText;
				}
			}

			return "nothing";

		}
	}
}
