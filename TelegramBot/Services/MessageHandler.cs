using System.Runtime.CompilerServices;
using Telegram.Bot.Types;

namespace TelegramBot.Services
{
  public class MessageHandler
  {
    public static async Task<string> Process(Message message, string currentCommand, CancellationToken ct)
    {
      switch (currentCommand)
      {
        case "1":
          return $"Длина сообщения: {message.Text.Length}";
        case "2":
          try
          {
            var parsed = await TryParseToList(message.Text, ct).ToListAsync(cancellationToken: ct);

            var sum = CalculateSum(parsed);

            return $"Сумма чисел: {sum}";
          }
          catch (ArgumentException ex)
          {
            Console.WriteLine(ex.Message);
            return ex.Message;
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
            return ex.Message;
          }
        default:
          return "";
      }
    }
    static decimal CalculateSum(List<decimal> mas) => mas.Sum();
    static async IAsyncEnumerable<decimal> TryParseToList(string message, [EnumeratorCancellation] CancellationToken ct)
    {
      var splittedText = message.Split(' ');

      foreach(var part in splittedText)
      {
        if(Decimal.TryParse(part, out decimal partResult))
        {
          yield return partResult;
        }
        else
        {
          throw new ArgumentException($"Невозможно преобразовать в массив чисел");
        }
      }
    }
  }
}
