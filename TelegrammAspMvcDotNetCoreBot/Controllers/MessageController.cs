using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegrammAspMvcDotNetCoreBot.Models;
using System.IO;
using System.Net;
using TelegrammAspMvcDotNetCoreBot.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "Method GET unuvalable";
        }

        // POST api/values
        [HttpPost]
        public async Task<OkResult> Post([FromBody]Update update)
		{
			if (update == null) return Ok();

            var commands = Bot.Commands;
            var message = update.Message;
            var botClient = await Bot.GetBotClientAsync();
			

			foreach (var command in commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, botClient);
					return Ok();
                }
            }
			

			if (UserController.CheckUser(message.Chat.Id))
			{
				if (UserController.CheckUserElements(message.Chat.Id, "university") == "")
				{
					UserController.EditUser(message.Chat.Id, "university", message.Text);

					//List<string> un = ScheduleController.GetFaculties(UserController.CheckUserElements(message.Chat.Id, "university"));

					//string[][] unn = new string[un.ToList().Count][];
					
					int count = 0;
					//foreach (string item in un)
					//{
					//	unn[count] = new string[] { item };
					//	count++;
					//}

					//await botClient.SendTextMessageAsync(message.Chat.Id, "Теперь выбери факультет", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: (Telegram.Bot.Types.ReplyMarkups.IReplyMarkup) KeybordController.GetKeyboard(unn, count));
					return Ok();

				}
				else if (UserController.CheckUserElements(message.Chat.Id, "faculty") == "")
				{
					UserController.EditUser(message.Chat.Id, "faculty", message.Text);

					//List<string> un = ScheduleController.GetCourses(UserController.CheckUserElements(message.Chat.Id, "university"), UserController.CheckUserElements(message.Chat.Id, "faculty"));

					//string[][] unn = new string[un.ToList().Count][];

					//int count = 0;
					//foreach (string item in un)
					//{
					//	unn[count] = new string[] { item };
					//	count++;
					//}
					
					//await botClient.SendTextMessageAsync(message.Chat.Id, "Теперь выбери курс", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: (Telegram.Bot.Types.ReplyMarkups.IReplyMarkup)KeybordController.GetKeyboard(unn, count));
					return Ok();

				}
				else if (UserController.CheckUserElements(message.Chat.Id, "course") == "")
				{
					UserController.EditUser(message.Chat.Id, "course", message.Text);

					//List<string> un = ScheduleController.GetGroups(UserController.CheckUserElements(message.Chat.Id, "university"), UserController.CheckUserElements(message.Chat.Id, "faculty"), int.Parse(UserController.CheckUserElements(message.Chat.Id, "course")));

					//string[][] unn = new string[un.ToList().Count][];

					//int count = 0;
					//foreach (string item in un)
					//{
					//	unn[count] = new string[] { item };
					//	count++;
					//}
					
					//await botClient.SendTextMessageAsync(message.Chat.Id, "Теперь выбери группу", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: (Telegram.Bot.Types.ReplyMarkups.IReplyMarkup)KeybordController.GetKeyboard(unn, count));
					return Ok();

				}
				else if (UserController.CheckUserElements(message.Chat.Id, "group") == "")
				{
					UserController.EditUser(message.Chat.Id, "group", message.Text);
					string[][] unn = new string[][]
					{
						new string[] {"Сегодня", "Завтра"}
					};

					
					await botClient.SendTextMessageAsync(message.Chat.Id, "Отлично, можем работать!", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: (Telegram.Bot.Types.ReplyMarkups.IReplyMarkup)KeybordController.GetKeyboard(unn, 1));
					return Ok();

				}
				else if (message.Text == "Сегодня")
				{
					int day;
					if ((int)DateTime.Now.DayOfWeek == 0)
						day = 7;
					else
					{
						day = (int)DateTime.Now.DayOfWeek;
					}

					//List<Para> listPar = ScheduleController.GetDaysShedule(UserController.CheckUserElements(message.Chat.Id, "university"), UserController.CheckUserElements(message.Chat.Id, "faculty"), int.Parse(UserController.CheckUserElements(message.Chat.Id, "course")), UserController.CheckUserElements(message.Chat.Id, "group"), 1, day);

					//string result = "";
					//foreach (Para item in listPar)
					//{
					//	result += item.time + "\n" + item.name + "\n" + item.room + "\n\n";
					//}
					//if (result != "")
					//	await botClient.SendTextMessageAsync(message.Chat.Id, result, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
					//else
					//	await botClient.SendTextMessageAsync(message.Chat.Id, "Учебы нет, отдыхай", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

					return Ok();

				}
				else if (message.Text == "Завтра")
				{
					//int day;
					//if ((int)DateTime.Now.DayOfWeek == 0)
					//	day = 1;
					//else
					//{
					//	if ((int)DateTime.Now.DayOfWeek == 6)
					//		day = 7;
					//	else
					//		day = ((int)DateTime.Now.DayOfWeek + 1) % 7;
					//}

					//List<Para> listPar = ScheduleController.GetDaysShedule(UserController.CheckUserElements(message.Chat.Id, "university"), UserController.CheckUserElements(message.Chat.Id, "faculty"), int.Parse(UserController.CheckUserElements(message.Chat.Id, "course")), UserController.CheckUserElements(message.Chat.Id, "group"), 1, day);

					//string result = "";
					//foreach (Para item in listPar)
					//{
					//	result += item.time + "\n" + item.name + "\n" + item.room + "\n\n";
					//}
					//if (result != "")
					//	await botClient.SendTextMessageAsync(message.Chat.Id, result, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
					//else
					//	await botClient.SendTextMessageAsync(message.Chat.Id, "Учебы нет, отдыхай", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

					return Ok();

				}
			}

			return Ok();
		}
	}
}
