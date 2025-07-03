namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomPaymentVM
    {
        public int InstanceId { get; set; }

        public string? Title { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; } = "USD";
    }
}

