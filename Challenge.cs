// DESAFIO: Sistema de Chat em Grupo
// PROBLEMA: Um aplicativo de mensagens tem usuários que precisam enviar mensagens para grupos,
// notificar quando entram/saem, e gerenciar permissões. O código atual faz cada usuário
// conhecer e se comunicar diretamente com todos os outros, criando acoplamento complexo

using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de chat onde usuários se comunicam em grupos
    // Problema: Comunicação direta entre usuários cria dependências complexas
    
    public class ChatUser
    {
        public string Name { get; set; }
        public bool IsMuted { get; set; }
        
        // Problema: Cada usuário mantém referências para todos os outros
        private List<ChatUser> _groupMembers;

        public ChatUser(string name)
        {
            Name = name;
            IsMuted = false;
            _groupMembers = new List<ChatUser>();
        }

        public void JoinGroup(List<ChatUser> members)
        {
            _groupMembers = members;
            
            // Problema: Usuário precisa notificar todos os outros diretamente
            foreach (var member in _groupMembers)
            {
                if (member != this)
                {
                    member.ReceiveNotification($"{Name} entrou no grupo");
                }
            }
            
            Console.WriteLine($"[{Name}] Entrou no grupo com {_groupMembers.Count} membros");
        }

        public void SendMessage(string message)
        {
            if (IsMuted)
            {
                Console.WriteLine($"[{Name}] ❌ Você está mutado");
                return;
            }

            Console.WriteLine($"[{Name}] Enviou: {message}");
            
            // Problema: Usuário precisa enviar mensagem para cada membro
            // Isso viola o princípio de responsabilidade única
            foreach (var member in _groupMembers)
            {
                if (member != this && !member.IsMuted)
                {
                    member.ReceiveMessage(Name, message);
                }
            }
        }

        public void SendPrivateMessage(ChatUser recipient, string message)
        {
            if (IsMuted)
            {
                Console.WriteLine($"[{Name}] ❌ Você está mutado");
                return;
            }

            // Problema: Lógica de mensagem privada duplicada
            Console.WriteLine($"[{Name}] Enviou mensagem privada para {recipient.Name}");
            recipient.ReceivePrivateMessage(Name, message);
        }

        public void LeaveGroup()
        {
            // Problema: Ao sair, precisa notificar todos manualmente
            foreach (var member in _groupMembers)
            {
                if (member != this)
                {
                    member.ReceiveNotification($"{Name} saiu do grupo");
                    member._groupMembers.Remove(this); // Modifica estado de outros objetos!
                }
            }
            
            _groupMembers.Clear();
            Console.WriteLine($"[{Name}] Saiu do grupo");
        }

        public void MuteUser(ChatUser target)
        {
            // Problema: Usuário pode modificar estado de outros diretamente
            // Sem validação de permissões
            target.IsMuted = true;
            Console.WriteLine($"[{Name}] Mutou {target.Name}");
            
            // E ainda precisa notificar todos
            foreach (var member in _groupMembers)
            {
                if (member != this && member != target)
                {
                    member.ReceiveNotification($"{target.Name} foi mutado por {Name}");
                }
            }
        }

        public void ReceiveMessage(string senderName, string message)
        {
            Console.WriteLine($"  → [{Name}] Recebeu de {senderName}: {message}");
        }

        public void ReceivePrivateMessage(string senderName, string message)
        {
            Console.WriteLine($"  → [{Name}] 🔒 Mensagem privada de {senderName}: {message}");
        }

        public void ReceiveNotification(string notification)
        {
            Console.WriteLine($"  → [{Name}] ℹ️ {notification}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Chat em Grupo ===\n");

            // Criando usuários
            var alice = new ChatUser("Alice");
            var bob = new ChatUser("Bob");
            var carlos = new ChatUser("Carlos");
            var diana = new ChatUser("Diana");

            // Problema: Precisa gerenciar lista manualmente
            var groupMembers = new List<ChatUser> { alice, bob, carlos, diana };

            Console.WriteLine("=== Usuários Entrando no Grupo ===");
            alice.JoinGroup(groupMembers);
            bob.JoinGroup(groupMembers);
            carlos.JoinGroup(groupMembers);
            diana.JoinGroup(groupMembers);

            Console.WriteLine("\n=== Conversação ===");
            alice.SendMessage("Olá, pessoal!");
            bob.SendMessage("Oi, Alice!");
            carlos.SendMessage("E aí!");

            Console.WriteLine("\n=== Mensagem Privada ===");
            alice.SendPrivateMessage(bob, "Bob, você viu o relatório?");

            Console.WriteLine("\n=== Moderação ===");
            alice.MuteUser(carlos);
            carlos.SendMessage("Ainda posso falar?"); // Não será enviado

            Console.WriteLine("\n=== Saindo do Grupo ===");
            diana.LeaveGroup();
            alice.SendMessage("Diana saiu");

            Console.WriteLine("\n=== PROBLEMAS ===");
            Console.WriteLine("✗ Acoplamento alto: cada usuário conhece todos os outros");
            Console.WriteLine("✗ Comunicação M×N: cada usuário envia para N-1 outros");
            Console.WriteLine("✗ Lógica de notificação duplicada em cada método");
            Console.WriteLine("✗ Usuários modificam estado de outros usuários diretamente");
            Console.WriteLine("✗ Difícil adicionar regras centralizadas (moderação, filtros)");
            Console.WriteLine("✗ Não há lugar único para implementar log de mensagens");
            Console.WriteLine("✗ Difícil adicionar novos tipos de interação");
            Console.WriteLine("✗ Gerenciamento de grupo espalhado entre usuários");

            Console.WriteLine("\n=== Requisitos Não Atendidos ===");
            Console.WriteLine("• Moderação centralizada com permissões");
            Console.WriteLine("• Log centralizado de todas as mensagens");
            Console.WriteLine("• Filtro de palavras proibidas");
            Console.WriteLine("• Rate limiting (limite de mensagens por minuto)");
            Console.WriteLine("• Histórico de mensagens");
            Console.WriteLine("• Notificações inteligentes");

            // Perguntas para reflexão:
            // - Como desacoplar objetos que precisam se comunicar?
            // - Como centralizar lógica de comunicação complexa?
            // - Como evitar comunicação direta entre muitos objetos?
            // - Como facilitar manutenção de interações entre componentes?
        }
    }
}
