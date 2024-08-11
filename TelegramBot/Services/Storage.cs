using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Interfaces;
using TelegramBot.Models;

namespace TelegramBot.Services
{
  public class Storage : IStorage
  {
    private readonly ConcurrentDictionary<long, Session> _sessions;
    public Storage()
    {
      _sessions = new();
    }
    public Session GetSession(long chatId)
    {
      if (_sessions.TryGetValue(chatId, out Session? value))
        return value;

      var newSession = new Session()
      {
        Command = "1"
      };

      _sessions.TryAdd(chatId, newSession);

      return newSession;
    }
  }
}
