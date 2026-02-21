using Mediator.Domain.Chat;

namespace Mediator.Domain.Abstractions
{
    public interface IChatMediator
    {
        void Join(ChatUser user);
        void Leave(ChatUser user);
        void SendMessage(string from, string message);
        void SendPrivateMessage(string from, string to, string message);
        void Mute(string moderator, string target);
    }
}