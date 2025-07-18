using JapaneseLearningPlatform.Models;
using System.Net;

public class VNPayService
{
    private readonly IConfiguration _config;

    public VNPayService(IConfiguration config)
    {
        _config = config;
    }
    public string CreatePaymentUrl(Order order, HttpContext httpContext)
    {
        var vnpay = _config.GetSection("Vnpay");
        var tick = DateTime.Now.Ticks.ToString();

        var url = vnpay["BaseUrl"]!;
        var returnUrl = vnpay["ReturnUrl"]!;

        // ✅ Không làm tròn, chỉ ép kiểu nếu bạn chắc TotalAmount đã là số nguyên VND
        long amountLong = (long)(order.TotalAmount * 100);
        var amount = amountLong.ToString();

        var tmnCode = vnpay["TmnCode"]!;
        var hashSecret = vnpay["HashSecret"]!;

        var vnp_Params = new SortedDictionary<string, string>
        {
            { "vnp_Version", vnpay["Version"]! },
            { "vnp_Command", vnpay["Command"]! },
            { "vnp_TmnCode", tmnCode },
            { "vnp_Amount", amount },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
            { "vnp_CurrCode", vnpay["CurrCode"]! },
            { "vnp_IpAddr", httpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
            { "vnp_Locale", vnpay["Locale"]! },
            { "vnp_OrderInfo", "Thanh toan khoa hoc Nihongo Sekai" },
            { "vnp_OrderType", "other" },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_TxnRef", tick }
        };

        // Tạo checksum
        var signData = string.Join("&", vnp_Params.Select(x =>
            $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));
        var vnp_SecureHash = VNPayHelper.HmacSHA512(hashSecret, signData);
        vnp_Params.Add("vnp_SecureHash", vnp_SecureHash);

        // Tạo URL đầy đủ với parameter đã encode
        var query = string.Join("&", vnp_Params.Select(x =>
            $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));

        return $"{url}?{query}";
    }
}
