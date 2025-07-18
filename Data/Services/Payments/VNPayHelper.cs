public static class VNPayHelper
{
    public static string HmacSHA512(string key, string input)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(System.Text.Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
