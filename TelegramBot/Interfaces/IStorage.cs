using TelegramBot.Models;

namespace TelegramBot.Interfaces
{
  public interface IStorage
  {
    Session GetSession(long chatId);
  }
}