using JapaneseLearningPlatform.Data.Enums;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class ClassroomInstanceSeed
    {
        public int TemplateIndex { get; set; }
        public int StartOffsetDays { get; set; }
        public int EndOffsetDays { get; set; }
        public TimeSpan ClassTime { get; set; }
        public int MaxCapacity { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public string GoogleMeetLink { get; set; } = "";
        public ClassroomStatus Status { get; set; }
    }
}
