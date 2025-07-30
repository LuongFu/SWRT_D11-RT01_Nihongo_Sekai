using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class PartnerDecisionVM
    {
        public int PartnerId { get; set; }

        // Dùng chung cho cả Approve và Reject
        public string Subject { get; set; } = "";

        public string Body { get; set; } = "";
    }

}
