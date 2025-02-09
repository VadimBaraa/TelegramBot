using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using TelegramBot_Module_11.Controllers;
using TelegramBot_Module_11.Services;

namespace TelegramBot_Module_11
{
    internal class Bot : BackgroundService
    {
        // Клиент к Telegram Bot API
        private ITelegramBotClient _telegramClient;

        // Контроллеры различных видов сообщений
        
        private TextMessageController _textMessageController;
        private VoiceMessageController _voiceMessageController;
        private DefaultMessageController _defaultMessageController;
        private InlineKeyboardController _inlineKeyboardController;
        private ChatStateService _chatStateService;

        public Bot(
            ITelegramBotClient telegramClient,
           
            TextMessageController textMessageController,
            VoiceMessageController voiceMessageController,
            DefaultMessageController defaultMessageController,
            InlineKeyboardController inlineKeyboardController,
            ChatStateService chatStateService)
        {
            _telegramClient = telegramClient;
            
            _textMessageController = textMessageController;
            _voiceMessageController = voiceMessageController;
            _defaultMessageController = defaultMessageController;
            _inlineKeyboardController = inlineKeyboardController;
            _chatStateService = chatStateService;
        }

      

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае - разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен.");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Обрабатываем нажатия на кнопки (CallbackQuery)
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            // Обрабатываем входящие сообщения
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message!;
                switch (message.Type)
                {
                    case MessageType.Text:
                        // Обработка команды /start
                        if (message.Text == "/start")
                        {
                            await _textMessageController.SendMainMenu(message.Chat.Id, cancellationToken);
                        }
                        else
                        {
                            // Обработка других текстовых сообщений
                            await _textMessageController.Handle(message, cancellationToken);
                        }
                        break;
                    default:
                        await _defaultMessageController.Handle(message, cancellationToken);
                        break;
                }
            }
        }



        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}