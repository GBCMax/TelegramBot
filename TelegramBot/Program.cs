using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TelegramBot.Configuration;
using TelegramBot.Controllers;
using TelegramBot.Interfaces;
using TelegramBot.Services;
using TelegramBot.Telegram;

internal class Program
{
  private static async Task Main(string[] args)
  {
    var host = new HostBuilder()
      .ConfigureServices((hostContext, services) => Configure(services))
      .UseConsoleLifetime()
      .Build();

    Console.WriteLine($"Сервис запущен");

    await host.RunAsync();

    Console.WriteLine($"Сервис остановлен");
  }
  static void Configure(IServiceCollection services)
  {
    AppSettings appSettings = BuildAppSettings();

    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));

    services.AddSingleton<IStorage, Storage>();

    services.AddTransient<TextMessageController>();

    services.AddTransient<InlineKeyboardController>();

    services.AddTransient<DefaultMessageController>();

    services.AddHostedService<TelegramBot.Telegram.TelegramBot>();
  }
  static AppSettings BuildAppSettings() =>
    new AppSettings()
    {
      BotToken = "7317595459:AAGx-qugq5ek0ka9Z7V381wXEKyMnAlCLZo"
    };
}