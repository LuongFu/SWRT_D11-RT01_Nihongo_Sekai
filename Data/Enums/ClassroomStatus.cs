using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum ClassroomStatus
    {
        [Display(Name = "Bản nháp")]
        Draft = 0,

        [Display(Name = "Đã công khai")]
        Published = 1,

        [Display(Name = "Đang diễn ra")]
        InProgress = 2,

        [Display(Name = "Đã kết thúc")]
        Completed = 3,

        [Display(Name = "Đã hủy")]
        Cancelled = 4
    }
}
