using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using TelegrammAspMvcDotNetCoreBot.Controllers;
using System.IO.Packaging;
using TelegrammAspMvcDotNetCoreBot.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{
    public static class ExcelParserController
	{

		private static string C(int number)
		{
			string result = "";

			if (number > 0)
			{
				if (number % 26 == 0)
				{
					result += C(number / 26 - 1);
					result += B(26);
				}
				else
				{
					result += C(number / 26);
					result += B(number % 26);
				}
			}

			return result;
		}

		private static char B(int number)
		{

			switch (number)
			{
				case 1: return 'A';
				case 2: return 'B';
				case 3: return 'C';
				case 4: return 'D';
				case 5: return 'E';
				case 6: return 'F';
				case 7: return 'G';
				case 8: return 'H';
				case 9: return 'I';
				case 10: return 'J';
				case 11: return 'K';
				case 12: return 'L';
				case 13: return 'M';
				case 14: return 'N';
				case 15: return 'O';
				case 16: return 'P';
				case 17: return 'Q';
				case 18: return 'R';
				case 19: return 'S';
				case 20: return 'T';
				case 21: return 'U';
				case 22: return 'V';
				case 23: return 'W';
				case 24: return 'X';
				case 25: return 'Y';
				case 26: return 'Z';
			}

			return 'w';

		}

		public static string GetCellValue(string fileName,
		string sheetName,
		string addressName)
		{
			string value = null;

			// Open the spreadsheet document for read-only access.
			using (var document = SpreadsheetDocument.Open("0мисис.xlsx", false))
			{
				// Retrieve a reference to the workbook part.
				WorkbookPart wbPart = document.WorkbookPart;

				// Find the sheet with the supplied name, and then use that 
				// Sheet object to retrieve a reference to the first worksheet.
				Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
				  Where(s => s.Name == sheetName).FirstOrDefault();

				// Throw an exception if there is no sheet.
				if (theSheet == null)
				{
					throw new ArgumentException("sheetName");
				}

				// Retrieve a reference to the worksheet part.
				WorksheetPart wsPart =
					(WorksheetPart)(wbPart.GetPartById(theSheet.Id));

				// Use its Worksheet property to get a reference to the cell 
				// whose address matches the address you supplied.
				Cell theCell = wsPart.Worksheet.Descendants<Cell>().
				  Where(c => c.CellReference == addressName).FirstOrDefault();

				// If the cell does not exist, return an empty string.
				if (theCell != null)
				{
					value = theCell.InnerText;

					// If the cell represents an integer number, you are done. 
					// For dates, this code returns the serialized value that 
					// represents the date. The code handles strings and 
					// Booleans individually. For shared strings, the code 
					// looks up the corresponding value in the shared string 
					// table. For Booleans, the code converts the value into 
					// the words TRUE or FALSE.
					if (theCell.DataType != null)
					{
						switch (theCell.DataType.Value)
						{
							case CellValues.SharedString:

								// For shared strings, look up the value in the
								// shared strings table.
								var stringTable =
									wbPart.GetPartsOfType<SharedStringTablePart>()
									.FirstOrDefault();

								// If the shared string table is missing, something 
								// is wrong. Return the index that is in
								// the cell. Otherwise, look up the correct text in 
								// the table.
								if (stringTable != null)
								{
									value =
										stringTable.SharedStringTable
										.ElementAt(int.Parse(value)).InnerText;
								}
								break;

							case CellValues.Boolean:
								switch (value)
								{
									case "0":
										value = "FALSE";
										break;
									default:
										value = "TRUE";
										break;
								}
								break;
						}
					}
				}
			}
			return value;
		}

		public static void Read(string FileName)
		{
			ScheduleController.CheckFile();
			ScheduleController.AddUniversity("мисис");
			ScheduleController.AddFaculty("мисис", FileName);

			for (int course = 1; course < 5; course++)
			{
				ScheduleController.AddCourse("мисис", FileName, course);

				string value = GetCellValue(FileName, course.ToString() + " курс", C(1) + "1");

				int group = 4;

				while (GetCellValue(FileName, course.ToString() + " курс", C(group) + "1") != "")
				{
					ScheduleController.AddGroup("мисис", FileName, course, GetCellValue(FileName, course.ToString() + " курс", C(group) + "1"));
					ScheduleController.AddWeek("мисис", FileName, course, GetCellValue(FileName, course.ToString() + " курс", C(group) + "1"), 1);
					ScheduleController.AddWeek("мисис", FileName, course, GetCellValue(FileName, course.ToString() + " курс", C(group) + "1"), 2);

					List<List<Para>> week1 = new List<List<Para>>();
					List<List<Para>> week2 = new List<List<Para>>();

					for (int podg = group + 2; podg <= group + 2; podg += 2)
					//	for (int podg = group; podg <= group + 2; podg += 2)
					{
						for (int dayofweek = 3; dayofweek < 100; dayofweek += 14)
						{
							//Console.WriteLine(ObjWorkSheet.Cells[dayofweek - 2 / 14, 1].ToString());

							List<Para> day1 = new List<Para>();
							List<Para> day2 = new List<Para>();

							for (int para = dayofweek; para < dayofweek + 14; para += 2)
							{
								if (GetCellValue(FileName, course.ToString() + " курс", C(podg) + para.ToString()) != "")
									day1.Add(new Para(name: GetCellValue(FileName, course.ToString() + " курс", C(podg) + para.ToString()), time: "", room: "", prepod: ""));
								
								if (GetCellValue(FileName, course.ToString() + " курс", C(podg) + (para + 1).ToString()) != "")
									day2.Add(new Para(name: GetCellValue(FileName, course.ToString() + " курс", C(podg) + (para + 1).ToString()), time: "", room: "", prepod: ""));
							}

							week1.Add(day1);
							week2.Add(day2);

							ScheduleController.AddSheduleWeek("мисис", FileName, course, GetCellValue(FileName, course.ToString() + " курс", C(group) + "1"), 1, week1);
							ScheduleController.AddSheduleWeek("мисис", FileName, course, GetCellValue(FileName, course.ToString() + " курс", C(group) + "1"), 2, week2);

						}


					}

					group += 4;
				}
			}
		}
	}
}
