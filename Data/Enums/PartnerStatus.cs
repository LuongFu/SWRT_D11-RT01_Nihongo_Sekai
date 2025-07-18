using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum PartnerStatus
    {
        [Display(Name = "Đang chờ duyệt")]
        Pending,

        [Display(Name = "Đã duyệt")]
        Approved,

        [Display(Name = "Từ chối")]
        Rejected
    }
}
