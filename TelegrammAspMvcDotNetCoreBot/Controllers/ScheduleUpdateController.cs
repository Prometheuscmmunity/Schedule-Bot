using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsQuery;
using System.IO;
using System.Net;
using System.Text;

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{
    public class ScheduleUpdateController
	{
		static void Update()
		{
			string mainUrl = "http://misis.ru/students/";

			CQ cq = CQ.Create(GetResponse(mainUrl));

			string[] files = new string[10];
			int count = 0;

			foreach (IDomObject obj in cq.Find(".XLSX a"))
			{
				Download("http://misis.ru" + obj.GetAttribute("href"), count + "мисис");
				count++;
			}

			foreach (IDomObject obj in cq.Find(".XLS a"))
			{
				Download("http://misis.ru" + obj.GetAttribute("href"), count + "мисис");
				count++;
			}
		}

		static string GetResponse(string uri)
		{
			StringBuilder sb = new StringBuilder();
			byte[] buf = new byte[8192];
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream resStream = response.GetResponseStream();
			int count = 0;
			do
			{
				count = resStream.Read(buf, 0, buf.Length);
				if (count != 0)
				{
					sb.Append(Encoding.Default.GetString(buf, 0, count));
				}
			}
			while (count > 0);
			return sb.ToString();
		}

		static void Download(string url, string name)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			Stream stream = resp.GetResponseStream();
			FileStream file = new FileStream(@"" + name + ".xls", FileMode.Create, FileAccess.Write);
			StreamWriter write = new StreamWriter(file);
			int b;
			for (int i = 0; ; i++)
			{
				b = stream.ReadByte();
				if (b == -1) break;
				write.Write((char)b);
			}
			write.Close();
			file.Close();
		}

	}
}
