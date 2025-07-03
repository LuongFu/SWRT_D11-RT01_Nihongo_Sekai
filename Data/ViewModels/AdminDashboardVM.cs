namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class AdminDashboardVM
    {
        // Metrics
        public int TotalUsers { get; set; }
        public int TotalLearners { get; set; }
        public int TotalPartners { get; set; }
        public int TotalAdmins { get; set; }

        public int TotalCourses { get; set; }
        public int TotalOrders { get; set; }
        public double TotalEarnings { get; set; }

        public int TotalClassrooms { get; set; }
        public int TotalEnrollments { get; set; }

        // Chart Data (for optional dynamic rendering)
        public List<string>? SalesMonths { get; set; } // e.g., ["Jan", "Feb", ...]
        public List<double>? MonthlySales { get; set; }

        public List<string>? OrderMonths { get; set; }
        public List<int>? MonthlyOrders { get; set; }
    }
}
