using Mediator.Application.Services;
using Mediator.Domain.Chat;

var mediator = new ChatRoomMediator();

var alice = new ChatUser("Alice");
var bob = new ChatUser("Bob");
var carlos = new ChatUser("Carlos");

mediator.Join(alice);
mediator.Join(bob);
mediator.Join(carlos);

alice.Send("Olá pessoal!");
bob.SendPrivate("Alice", "Oi!");
alice.Mute("Carlos");

carlos.Send("Ainda posso falar?");