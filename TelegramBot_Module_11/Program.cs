using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot_Module_11;
using TelegramBot_Module_11.Configurations;
using TelegramBot_Module_11.Controllers;
using TelegramBot_Module_11.Services;

namespace UtilityBot
{
    static class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Starting Service");
            await host.RunAsync();
            Console.WriteLine("Service stopped");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddSingleton<IFileHandler, AudioFileHandler>();

            services.AddSingleton<ChatStateService>();

            services.AddTransient<DefaultMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>(); 
           
            
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddHostedService<Bot>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                DownloadsFolder = "D:\\downloads\\TGBOT",
                BotToken = "7498380703:AAEv6EbrlMKKhQTJBcS-K33QBA3ZVM4PSb8",
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
                OutputAudioFormat = "wav", 
                InputAudioBitrate = 48000,
            };
        }
    }
}