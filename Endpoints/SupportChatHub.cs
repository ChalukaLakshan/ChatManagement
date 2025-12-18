using Microsoft.AspNetCore.SignalR;
using Service;

namespace ChatManagement.Endpoints
{
    public class SupportChatHub : Hub
    {
        IBusinessServiceProvider _businessServiceProvider;
        public SupportChatHub(IBusinessServiceProvider businessServiceProvider)
        {
            _businessServiceProvider = businessServiceProvider;
        }

        public async Task<int> SendMessageToAgent(
        string sessionId,
        int chatId,
        string message)
        {
            await Clients.Group($"user-{sessionId}")
                .SendAsync("ReceiveMessage", new
                {
                    ChatId = chatId,
                    Message = message,
                    From = "User"
                });
           return await _businessServiceProvider.ChatService!.SaveMessageAsync(sessionId, chatId, message);
        }
        
        public async Task<int> SendMessageToUser(
            string sessionId,
            int chatId,
            string message)
        {
            await Clients.Client(sessionId)
                .SendAsync("ReceiveMessage", new
                {
                    ChatId = chatId,
                    Message = message,
                    From = "Agent"
                });

            return await _businessServiceProvider.ChatService!.SaveAgentMessageAsync(sessionId, chatId, message);
        }
    }
}
