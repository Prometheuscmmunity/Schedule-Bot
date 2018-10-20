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
					await botClient.SendTextMessageAsync(message.Chat.Id, "Теперь выбери факультет", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
					return Ok();

				}
				else if (UserController.CheckUserElements(message.Chat.Id, "faculty") == "")
				{

					UserController.EditUser(message.Chat.Id, "faculty", message.Text);
					await botClient.SendTextMessageAsync(message.Chat.Id, "Теперь выбери курс", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
					return Ok();

				}
				else if (UserController.CheckUserElements(message.Chat.Id, "course") == "")
				{

					UserController.EditUser(message.Chat.Id, "course", message.Text);
					await botClient.SendTextMessageAsync(message.Chat.Id, "Теперь выбери группу", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
					return Ok();

				}
				else if (UserController.CheckUserElements(message.Chat.Id, "group") == "")
				{

					UserController.EditUser(message.Chat.Id, "group", message.Text);
					await botClient.SendTextMessageAsync(message.Chat.Id, "Отлично, можем работать!", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
					return Ok();

				}
			}

			return Ok();
		}
	}
}
