using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using TelegramBot.Controllers;

namespace TelegramBot.Telegram
{
  public class TelegramBot : BackgroundService
  {
    private readonly ITelegramBotClient _tgClient;
    private TextMessageController _textMsgController;
    private readonly DefaultMessageController _defaultMsgController;
    private readonly InlineKeyboardController _keyboardController;
    public TelegramBot(ITelegramBotClient tgClient, TextMessageController textMsgController, DefaultMessageController defaultMsgController, InlineKeyboardController keyboardController)
    {
      _tgClient = tgClient;
      _textMsgController = textMsgController;
      _defaultMsgController = defaultMsgController;
      _keyboardController = keyboardController;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _tgClient.StartReceiving(
        HandleUpdateAsync,
        HandleErrorAsync,
        new ReceiverOptions() { AllowedUpdates = { } },
        cancellationToken: stoppingToken);

      Console.WriteLine($"Бот запущен");
    }
    private async Task HandleUpdateAsync(ITelegramBotClient telegramClient, Update update, CancellationToken ct)
    {
      if (update.Type == UpdateType.CallbackQuery)
      {
        await _keyboardController.Handle(update.CallbackQuery, ct);

        return;
      }

      if (update.Type == UpdateType.Message)
      {
        switch (update.Message!.Type)
        {
          case MessageType.Text:
            await _textMsgController.Handle(update.Message, ct);
            return;
          default:
            await _defaultMsgController.Handle(update.Message, ct);
            return;
        }
      }
    }

    private Task HandleErrorAsync(ITelegramBotClient telegramClient, Exception ex, CancellationToken ct)
    {
      var errorMessage = ex switch
      {
        ApiRequestException apiRequestEx
          => $"Telegram API Error:\n[{apiRequestEx.ErrorCode}]\n{apiRequestEx.Message}",
        _ => ex.ToString()
      };

      Console.WriteLine(errorMessage);

      Console.WriteLine("Ожидаем 10 секунд перед повторным подключением");

      Thread.Sleep(10000);

      return Task.CompletedTask;
    }
  }
}
