using JapaneseLearningPlatform.Models;
using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomInstanceWithTemplateVM
    {
        public ClassroomInstance Instance { get; set; }
        public ClassroomTemplate Template { get; set; }

        // Optional - thống kê nhanh
        public int EnrollmentCount { get; set; }
        public bool IsFull => Instance != null && EnrollmentCount >= Instance.MaxCapacity;
        // Convenience fields (đã tách từ Instance)
        public string Title => Template?.Title ?? "Untitled";
        public string? ImageURL => Template?.ImageURL;
        public ClassroomStatus Status => Instance.Status;
        public bool IsPaid => Instance.IsPaid;
        public decimal Price => Instance.Price;
        public DateTime StartDate => Instance.StartDate;
        public DateTime EndDate => Instance.EndDate;
        public TimeSpan ClassTime => Instance.ClassTime;
    }
}
