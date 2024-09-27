using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using TelegramBot_Module_11.Services;

namespace TelegramBot_Module_11.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly ChatStateService _chatStateService;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, ChatStateService chatStateService)
        {
            _telegramClient = telegramBotClient;
            _chatStateService = chatStateService;
        }

        public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
        {
            switch (callbackQuery.Data)
            {
                case "count_chars":
                    _chatStateService.SetState(callbackQuery.Message.Chat.Id, "count_chars");
                    await _telegramClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Пожалуйста, отправьте текст для подсчета символов.", cancellationToken: ct);
                    break;
                case "sum_numbers":
                    _chatStateService.SetState(callbackQuery.Message.Chat.Id, "sum_numbers");
                    await _telegramClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Пожалуйста, отправьте числа через пробел для вычисления суммы.", cancellationToken: ct);
                    break;
                default:
                    await _telegramClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Неизвестная команда.", cancellationToken: ct);
                    break;
            }
        }
    }


}