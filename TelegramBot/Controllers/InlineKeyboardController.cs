using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;

namespace TelegramBot.Controllers
{
  [ApiController]
  public class InlineKeyboardController : ControllerBase
  {
    private readonly ITelegramBotClient _botClient;
    private readonly IStorage _storage;
    public InlineKeyboardController(ITelegramBotClient botClient, IStorage storage)
    {
      _botClient = botClient;
      _storage = storage;
    }
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
      if (callbackQuery?.Data is null)
        return;

      _storage.GetSession(callbackQuery.From.Id).Command = callbackQuery.Data;

      string currentCommand = callbackQuery.Data switch
      {
        "1" => "Подсчет количества символов в тексте",
        "2" => "Вычисление суммы чисел",
        _ => String.Empty
      };

      await _botClient.SendTextMessageAsync(callbackQuery.From.Id,
          $"<b>Текущая команда - {currentCommand}.{Environment.NewLine}</b>" +
          $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
    }
  }
}
