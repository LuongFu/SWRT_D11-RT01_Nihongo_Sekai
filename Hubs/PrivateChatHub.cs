using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Hubs
{
    public class PrivateChatHub : Hub
    {
        private readonly AppDbContext _context;

        public PrivateChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendPrivateMessage(int classroomId, string targetUserId, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var senderId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(senderId)) return;

            bool isValid = await _context.ClassroomInstances
                .AnyAsync(ci => ci.Id == classroomId &&
                                (ci.Template.PartnerId == senderId || ci.Enrollments.Any(e => e.LearnerId == senderId)) &&
                                (ci.Template.PartnerId == targetUserId || ci.Enrollments.Any(e => e.LearnerId == targetUserId)));

            if (!isValid) return;

            var sender = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == senderId);
            if (sender == null) return;

            var chatMessage = new PrivateChatMessage
            {
                ClassroomInstanceId = classroomId,
                UserId = senderId,
                TargetUserId = targetUserId,
                Message = message.Trim(),
                SentAt = DateTime.UtcNow
            };
            _context.PrivateChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            string displayName = string.IsNullOrEmpty(sender.FullName) ? sender.Email : sender.FullName;
            string avatarUrl = string.IsNullOrEmpty(sender.ProfilePicturePath)
                ? "/uploads/profile/default-img.jpg"
                : sender.ProfilePicturePath;

            string groupName = GetPrivateChatGroup(classroomId, senderId, targetUserId);

            await Clients.Group(groupName).SendAsync("ReceivePrivateMessage",
                displayName,
                message.Trim(),
                chatMessage.SentAt.ToLocalTime().ToString("HH:mm dd/MM"),
                senderId,
                avatarUrl);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var classroomId = httpContext?.Request.Query["classroomId"].ToString();
            var targetUserId = httpContext?.Request.Query["targetUserId"].ToString();
            var userId = Context.UserIdentifier;

            if (int.TryParse(classroomId, out int classId) &&
                !string.IsNullOrEmpty(targetUserId) &&
                !string.IsNullOrEmpty(userId))
            {
                string groupName = GetPrivateChatGroup(classId, userId, targetUserId);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var classroomId = httpContext?.Request.Query["classroomId"].ToString();
            var targetUserId = httpContext?.Request.Query["targetUserId"].ToString();
            var userId = Context.UserIdentifier;

            if (int.TryParse(classroomId, out int classId) &&
                !string.IsNullOrEmpty(targetUserId) &&
                !string.IsNullOrEmpty(userId))
            {
                string groupName = GetPrivateChatGroup(classId, userId, targetUserId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private string GetPrivateChatGroup(int classroomId, string user1, string user2)
        {
            return $"private_{classroomId}_{(string.CompareOrdinal(user1, user2) < 0 ? $"{user1}_{user2}" : $"{user2}_{user1}")}";
        }
    }
}
