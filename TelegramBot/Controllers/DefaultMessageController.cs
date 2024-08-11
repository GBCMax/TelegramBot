using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Web.Mvc;

namespace TelegramBot.Controllers
{
  [ApiController]
  public class DefaultMessageController : Controller
  {
    private readonly ITelegramBotClient _tgClient;
    public DefaultMessageController(ITelegramBotClient tgClient)
    {
      _tgClient = tgClient;
    }
    public async Task Handle(Message message, CancellationToken ct)
    {
      Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
      await _tgClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение не поддерживаемого формата", cancellationToken: ct);
    }
  }
}
