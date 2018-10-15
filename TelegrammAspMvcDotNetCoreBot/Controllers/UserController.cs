using System;
using System.Xml.Linq;
using System.Xml;

namespace XmlUsers
{
    class User
    {
        public static void CreateUser(long UserId)
        {
            XDocument xDoc = XDocument.Load("users.xml");
            XElement xRoot = xDoc.Element("users");
            XElement newUser = new XElement("user",
                new XAttribute("id", UserId),
                new XElement("university", ""), 
                new XElement("faculty", ""),
                new XElement("course", ""),
                new XElement("group", ""));
            xDoc.Save("users.xml");
        }

        public static bool CheckUser(long UserId) //Проверка существования пользователя
        {
            //Делаю через System.Xml
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlNode node = xRoot.SelectSingleNode("user[@id='" + UserId + "']");
            if (node != null)
            {
                return true;
            }
            return false;
        }

        public static void RecreateUser(long UserId)
        {

            //Делаю через System.Xml
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlNode node = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

            xRoot.RemoveChild(node);
            xDoc.Save("users.xml");

            CreateUser(UserId);
        }

        public static void EditUser(long UserId, string type, string param)
        {
            //Делаю через System.Xml
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlNode node = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

            foreach (XmlNode find in node.ChildNodes)
            {
                if (find.Name == type)
                {
                    node.RemoveChild(find);
                    XmlElement el = xDoc.CreateElement(type);
                    XmlText text = xDoc.CreateTextNode(param);
                    node.AppendChild(el);
                    el.AppendChild(text);
                    xDoc.Save("users.xml");
                    break;
                }
            }
        }
        public static string CheckUserElements(long UserId, string type) //ЧТО МЫ ВЫВОДИМ? Должны ли мы искать по определенному type? 
        {
            //Делаю через System.Xml
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlNode node = xRoot.SelectSingleNode("user[@id='" + UserId + "']");

            string ans ="";
            foreach (XmlNode find in node.ChildNodes) //Запутался
            {
                if (find.Name == type)
                {
                    if (find.InnerText != "")
                    {
                        ans = "";
                    }
                    else
                    {
                        ans = find.InnerText;
                    }
                    return ans;
                }
            }
            return ans;
        }
    }
}
