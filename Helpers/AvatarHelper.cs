using System;

namespace JapaneseLearningPlatform.Helpers
{
    public static class AvatarHelper
    {
        // List of available Dicebear sprite styles
        private static readonly string[] Styles = new[]
        {
            "avataaars",
            "micah",
            "identicon",
            "jdenticon",
            "gridy",
            "bottts",
            "croodles",
            "pixel-art"
        };

        private static readonly Random _rnd = new Random();

        /// <summary>
        /// Generate a random Dicebear avatar URL based on a seed (e.g., user ID or name).
        /// </summary>
        /// <param name="seed">Any string to seed the avatar generation (e.g., user.FullName or user.Id).</param>
        /// <param name="size">Size in pixels (width & height) for the avatar.</param>
        /// <returns>URL to the generated avatar image.</returns>
        public static string GetAvatarUrl(string seed, int size = 80)
        {
            if (string.IsNullOrWhiteSpace(seed))
                seed = Guid.NewGuid().ToString();

            // pick a style deterministically based on seed hash
            int hash = seed.GetHashCode();
            var style = Styles[Math.Abs(hash) % Styles.Length];

            // optional: pick a random background palette or other query params
            var bgColors = new[] { "b6e3f4", "ffd93d", "ffdfbf", "c0aede", "bfe6ba" };
            var bg = bgColors[Math.Abs(hash / Styles.Length) % bgColors.Length];

            return $"https://api.dicebear.com/7.x/{style}/svg?seed={Uri.EscapeDataString(seed)}&backgroundColor={bg}&size={size}";
        }
    }
}
