using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class UserSeed
    {
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
        public bool IsBanned { get; set; }
    }
}
