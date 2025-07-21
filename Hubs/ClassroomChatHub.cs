using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Hubs
{
    public class ClassroomChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ClassroomChatHub(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gửi tin nhắn đến tất cả thành viên trong group của classroom và lưu vào DB.
        /// </summary>
        public async Task SendMessage(int classroomId, string _, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            // Lấy userId từ Context
            var userId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("⚠️ User is not authenticated in SignalR context.");
                return;
            }

            try
            {
                // Lấy thông tin user từ DB
                var user = await _context.Users.AsNoTracking()
                                               .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) return;

                var displayName = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.Email;
                var avatarUrl = string.IsNullOrEmpty(user.ProfilePicturePath)
                    ? "/uploads/profile/default-img.jpg"
                    : user.ProfilePicturePath;

                // 1. Lưu tin nhắn vào DB
                var chatMessage = new ClassroomChatMessage
                {
                    ClassroomInstanceId = classroomId,
                    UserId = userId,
                    Message = message.Trim(),
                    SentAt = DateTime.UtcNow
                };

                _context.ClassroomChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();

                // 2. Gửi tin nhắn kèm avatar
                await Clients.Group($"classroom_{classroomId}")
                    .SendAsync("ReceiveMessage",
                                displayName,
                                message.Trim(),
                                chatMessage.SentAt.ToLocalTime().ToString("HH:mm dd/MM"),
                                userId,
                                avatarUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClassroomChatHub.SendMessage] Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Khi kết nối mới được tạo, tự động thêm vào group classroom tương ứng.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            var classroomId = Context.GetHttpContext()?.Request.Query["classroomId"].ToString();
            if (!string.IsNullOrEmpty(classroomId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"classroom_{classroomId}");
                Console.WriteLine($"✅ User joined classroom_{classroomId}");
            }
            else
            {
                Console.WriteLine("⚠️ classroomId is missing in connection query.");
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Khi kết nối bị ngắt, tự động rời group classroom.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var classroomId = Context.GetHttpContext()?.Request.Query["classroomId"].ToString();
            if (!string.IsNullOrEmpty(classroomId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"classroom_{classroomId}");
                Console.WriteLine($"ℹ️ User left classroom_{classroomId}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
