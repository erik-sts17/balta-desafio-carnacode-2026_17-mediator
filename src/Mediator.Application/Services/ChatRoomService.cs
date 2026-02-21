using Mediator.Domain.Abstractions;
using Mediator.Domain.Chat;

namespace Mediator.Application.Services
{
    public class ChatRoomMediator : IChatMediator
    {
        private readonly List<ChatUser> _users = new();

        public void Join(ChatUser user)
        {
            _users.Add(user);
            user.SetMediator(this);
            Broadcast("Sistema", $"{user.Name} entrou no grupo");
        }

        public void Leave(ChatUser user)
        {
            _users.Remove(user);
            Broadcast("Sistema", $"{user.Name} saiu do grupo");
        }

        public void SendMessage(string from, string message)
        {
            var sender = _users.First(u => u.Name == from);

            if (sender.IsMuted)
            {
                sender.Notify("Você está mutado");
                return;
            }

            foreach (var user in _users.Where(u => u != sender))
                user.Receive(from, message);
        }

        public void SendPrivateMessage(string from, string to, string message)
        {
            var recipient = _users.FirstOrDefault(u => u.Name == to);
            recipient?.Receive(from, $"Private - {message}");
        }

        public void Mute(string moderator, string target)
        {
            var user = _users.FirstOrDefault(u => u.Name == target);
            if (user == null) return;

            user.Mute();
            Broadcast("Sistema", $"{target} foi mutado por {moderator}");
        }

        private void Broadcast(string from, string message)
        {
            foreach (var user in _users)
                user.Receive(from, message);
        }
    }
}