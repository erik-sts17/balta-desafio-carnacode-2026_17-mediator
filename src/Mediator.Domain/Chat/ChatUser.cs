using Mediator.Domain.Abstractions;

namespace Mediator.Domain.Chat
{
    public class ChatUser(string name)
    {
        private IChatMediator? _mediator;

        public string Name { get; } = name;
        public bool IsMuted { get; internal set; }

        public void Mute() 
        {
            IsMuted = true;
        }

        public void SetMediator(IChatMediator mediator)
            => _mediator = mediator;

        public void Send(string message)
            => _mediator?.SendMessage(Name, message);

        public void SendPrivate(string to, string message)
            => _mediator?.SendPrivateMessage(Name, to, message);

        public void Join()
            => _mediator?.Join(this);

        public void Leave()
            => _mediator?.Leave(this);

        public void Mute(string target)
            => _mediator?.Mute(Name, target);

        public void Receive(string from, string message)
            => Console.WriteLine($"[{Name}] {from}: {message}");

        public void Notify(string message)
            => Console.WriteLine($"[{Name}] - {message}");
    }
}