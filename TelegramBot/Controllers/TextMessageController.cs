using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using TelegramBot_Module_11.Services;

namespace TelegramBot_Module_11.Services
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly ChatStateService _chatStateService;

        public TextMessageController(ITelegramBotClient telegramBotClient, ChatStateService chatStateService)
        {
            _telegramClient = telegramBotClient;
            _chatStateService = chatStateService;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            string currentState = _chatStateService.GetState(message.Chat.Id);

            if (message.Text.StartsWith("/start"))
            {
                await SendMainMenu(message.Chat.Id, ct);
            }
            else if (currentState == "count_chars")
            {
                // Если состояние подсчета символов
                int charCount = message.Text.Length;
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"В вашем сообщении {charCount} символов.", cancellationToken: ct);
            }
            else if (currentState == "sum_numbers")
            {
                // Если состояние вычисления суммы чисел
                var numbers = message.Text.Split(' ')
                                           .Select(num => int.TryParse(num, out int n) ? n : 0);
                int sum = numbers.Sum();
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {sum}", cancellationToken: ct);
            }
            else
            {
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, выберите действие с помощью кнопок или введите текст.", cancellationToken: ct);
            }
        }

        public async Task SendMainMenu(long chatId, CancellationToken ct)
        {
            var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Подсчитать символы", "count_chars"),
                InlineKeyboardButton.WithCallbackData("Вычислить сумму чисел", "sum_numbers")
            }
        };

            await _telegramClient.SendTextMessageAsync(chatId, "Выберите действие:", cancellationToken: ct, replyMarkup: new InlineKeyboardMarkup(buttons));
        }
    }



}
