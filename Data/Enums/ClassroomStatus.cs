namespace JapaneseLearningPlatform.Data.Enums
{
    public enum ClassroomStatus
    {
        Draft = 0,         // Đang được tạo, chưa công bố
        Published = 1,     // Đã công khai, có thể hiển thị trên trang Guest/Learner
        InProgress = 2,    // Đang diễn ra (tự động hoặc thủ công cập nhật)
        Completed = 3,     // Đã kết thúc
        Cancelled = 4      // Bị hủy
    }

}
