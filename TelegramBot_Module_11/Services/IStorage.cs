using TelegramBot_Module_11.Models;

namespace TelegramBot_Module_11.Services
{
    public interface IStorage
    {

       Session GetSession(long chatId);
    }
}
