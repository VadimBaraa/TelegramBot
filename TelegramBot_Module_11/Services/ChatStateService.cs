using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot_Module_11.Services
{
    public class ChatStateService
    {
        private readonly Dictionary<long, string> _chatStates = new Dictionary<long, string>();

        public void SetState(long chatId, string state)
        {
            if (_chatStates.ContainsKey(chatId))
            {
                _chatStates[chatId] = state;
            }
            else
            {
                _chatStates.Add(chatId, state);
            }
        }

        public string GetState(long chatId)
        {
            if (_chatStates.TryGetValue(chatId, out string state))
            {
                return state;
            }
            return "default"; // По умолчанию, если нет состояния
        }
    }
}
