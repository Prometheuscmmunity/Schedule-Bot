using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegrammAspMvcDotNetCoreBot.Controllers;

namespace TelegrammAspMvcDotNetCoreBot.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => @"/start";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.TextMessage)
                return false;

            return message.Text.Contains(this.Name);
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
		{
			string[][] buttons = new string[][]
			{
				new string[] {"МИСиС"}
			};

			var chatId = message.Chat.Id;

			UserController.CheckUser(chatId);

			//await botClient.SendTextMessageAsync(chatId, "Hallo I'm ASP.NET Core Bot and I made by Mr.Robot", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
			await botClient.SendTextMessageAsync(chatId, "Привет, выбери свой университет", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: (Telegram.Bot.Types.ReplyMarkups.IReplyMarkup) KeybordController.GetKeyboard(buttons, 1));
		}
	}
}
