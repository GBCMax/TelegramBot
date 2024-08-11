using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Interfaces;
using TelegramBot.Services;

namespace TelegramBot.Controllers
{
  [ApiController]
  public class TextMessageController : Controller
  {
    private readonly ITelegramBotClient _tgClient;
    private readonly IStorage _storage;
    public TextMessageController(ITelegramBotClient tgClient, IStorage storage)
    {
      _tgClient = tgClient;
      _storage = storage;
    }
    public async Task Handle(
      Message message, 
      CancellationToken ct)
    {
      switch (message.Text)
      {
        case "/start":
          var buttons = new List<InlineKeyboardButton[]>
          {
            ([
              InlineKeyboardButton.WithCallbackData($"Подсчет количества символов в тексте", $"1"),
              InlineKeyboardButton.WithCallbackData($"Вычисление суммы чисел", $"2")
            ])
          };
          await _tgClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Бот умеет подсчитывать количество символов в тексте, а также вычислять сумму чисел.</b> {Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
          break;

        default:
          var currentCommand = _storage.GetSession(message.Chat.Id).Command;
          await _tgClient.SendTextMessageAsync(message.Chat.Id, await MessageHandler.Process(message, currentCommand, ct), cancellationToken: ct);
          break;
      }
    }
  }
}
