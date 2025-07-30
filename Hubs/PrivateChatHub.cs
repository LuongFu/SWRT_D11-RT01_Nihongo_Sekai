using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Hubs
{
    [Authorize]
    public class PrivateChatHub : Hub
    {
        private readonly AppDbContext _context;

        public PrivateChatHub(AppDbContext context)
        {
            _context = context;
        }

        // Gửi tin nhắn riêng
        public async Task SendPrivateMessage(int classroomId, string senderId, string targetUserId, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {
                // Lưu tin nhắn vào DB
                var newMessage = new PrivateChatMessage
                {
                    ClassroomInstanceId = classroomId,
                    UserId = senderId,
                    TargetUserId = targetUserId,
                    Message = message,
                    SentAt = DateTime.UtcNow
                };

                _context.PrivateChatMessages.Add(newMessage);
                await _context.SaveChangesAsync();

                var timeSent = newMessage.SentAt.ToLocalTime().ToString("HH:mm dd/MM");

                // Gửi tin nhắn về cho người gửi (isOwn = true)
                await Clients.User(senderId).SendAsync("ReceivePrivateMessage", senderId, message, timeSent, true);

                // Gửi cho người nhận (isOwn = false)
                await Clients.User(targetUserId).SendAsync("ReceivePrivateMessage", senderId, message, timeSent, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PrivateChatHub] Error: {ex.Message}");
                throw;
            }
        }

        // Thông báo "đang gõ..."
        public async Task NotifyTyping(int classroomId, string senderId, string targetUserId)
        {
            await Clients.User(targetUserId).SendAsync("UserTyping", senderId);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"[PrivateChatHub] User connected: {userId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"[PrivateChatHub] User disconnected: {userId}");
            await base.OnDisconnectedAsync(exception);
        }
    }

    // Custom UserIdProvider để SignalR map đúng ApplicationUser.Id
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
