using JapaneseLearningPlatform.Models;
using System.Net;
using Microsoft.AspNetCore.Http;

public class VNPayService
{
    private readonly IConfiguration _config;

    public VNPayService(IConfiguration config)
    {
        _config = config;
    }

    // Thêm txnRef vào tham số để tạo URL
    public string CreatePaymentUrl(Order order, HttpContext httpContext, string txnRef)
    {
        var vnpay = _config.GetSection("Vnpay");
        var url = vnpay["BaseUrl"]!;
        var returnUrl = vnpay["ReturnUrl"]!;

        // Nếu order.TotalAmount là số VND (không phải USD), bỏ *100
        long amountLong = (long)(order.TotalAmount * 100);
        var amount = amountLong.ToString();

        var tmnCode = vnpay["TmnCode"]!;
        var hashSecret = vnpay["HashSecret"]!;

        var vnp_Params = new SortedDictionary<string, string>
        {
            ["vnp_Version"] = vnpay["Version"]!,
            ["vnp_Command"] = vnpay["Command"]!,
            ["vnp_TmnCode"] = tmnCode,
            ["vnp_Amount"] = amount,
            ["vnp_CreateDate"] = DateTime.Now.ToString("yyyyMMddHHmmss"),
            ["vnp_CurrCode"] = vnpay["CurrCode"]!,
            ["vnp_IpAddr"] = httpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
            ["vnp_Locale"] = vnpay["Locale"]!,
            ["vnp_OrderInfo"] = "Thanh toan khoa hoc Nihongo Sekai",
            ["vnp_OrderType"] = "other",
            ["vnp_ReturnUrl"] = returnUrl,
            ["vnp_TxnRef"] = txnRef
        };

        // Tạo chữ ký
        var signData = string.Join("&", vnp_Params.Select(x =>
            $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));
        var vnp_SecureHash = VNPayHelper.HmacSHA512(hashSecret, signData);
        vnp_Params.Add("vnp_SecureHash", vnp_SecureHash);

        // Tạo query và trả URL
        var query = string.Join("&", vnp_Params.Select(x =>
            $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));

        return $"{url}?{query}";
    }
}
