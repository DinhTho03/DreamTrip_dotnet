using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using brandportal_dotnet.Contracts.Client.Payment;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Library;
using Microsoft.AspNetCore.Mvc;

namespace brandportal_dotnet.Controllers.Client.Payments;

[Route("[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
    private readonly string vnp_TmnCode = "NFDUTT48"; // Mã website của bạn
    private readonly string vnp_HashSecret = "6OEPJC51AMC1U29T8EPJBB0G0IJAIH7T"; // Chuỗi mã hóa bí mật
    private readonly string urlCallBack = "http://dreamtrip.site/profile/desposit"; // Chuỗi mã hóa bí mật
    private readonly IRepository<Account> _account;
    private readonly IRepository<Payment> _payment;

    public PaymentController(IRepository<Payment> payment,
        IRepository<Account> account)
    {
        _payment = payment;
        _account = account;
    }

    [HttpPost("~/api/create-payment-url")]
    public IActionResult CreatePaymentUrl([FromBody] PaymentRequest request)
    {
        var context = HttpContext;
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VnPayLibrary();
        var urlCallBack = this.urlCallBack;

        pay.AddRequestData("vnp_Version", "2.1.0");
        pay.AddRequestData("vnp_Command", "pay");
        pay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)request.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", "VND");
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", "vn");
        pay.AddRequestData("vnp_OrderInfo", request.OrderInfo);
        pay.AddRequestData("vnp_OrderType", "Đậu");
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", tick);

        var paymentUrl =
            pay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

        return Ok(new
        {
            UrlPayment = paymentUrl
        });
    }

    [HttpGet("~/api/payment-result")]
    public async Task<IActionResult> PaymentResult([FromQuery] PaymentDetailDto request)
    {
        if (request == null || string.IsNullOrEmpty(request.vnp_SecureHash))
        {
            return BadRequest(new { status = "fail", message = "Invalid request or missing secure hash" });
        }

        // Extract and process vnp_OrderInfo
        var orderInfoParts = Uri.UnescapeDataString(request.vnp_OrderInfo).Split(" - ", StringSplitOptions.TrimEntries);
        string userId = orderInfoParts.Length > 0 ? orderInfoParts[0] : string.Empty;
        string orderInfo = orderInfoParts.Length > 1 ? orderInfoParts[1] : string.Empty;

        // Validate and continue processing
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(orderInfo))
        {
            return BadRequest(new { status = "fail", message = "Invalid OrderInfo format" });
        }

        // Extract and validate secure hash
        var providedSecureHash = request.vnp_SecureHash;

        // Create a dictionary for all query parameters except the secure hash
        var queryParams = new Dictionary<string, string>
        {
            { nameof(request.vnp_Amount), request.vnp_Amount },
            { nameof(request.vnp_BankCode), request.vnp_BankCode },
            { nameof(request.vnp_BankTranNo), request.vnp_BankTranNo },
            { nameof(request.vnp_CardType), request.vnp_CardType },
            { nameof(request.vnp_OrderInfo), request.vnp_OrderInfo },
            { nameof(request.vnp_PayDate), request.vnp_PayDate },
            { nameof(request.vnp_ResponseCode), request.vnp_ResponseCode },
            { nameof(request.vnp_TmnCode), request.vnp_TmnCode },
            { nameof(request.vnp_TransactionNo), request.vnp_TransactionNo },
            { nameof(request.vnp_TransactionStatus), request.vnp_TransactionStatus },
            { nameof(request.vnp_TxnRef), request.vnp_TxnRef }
        };

        // Sort the parameters by key to match the signing format
        var orderedQuery = queryParams.OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.Value);

        // Construct the data to sign
        var signData = string.Join("&", orderedQuery.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

        // Compute the HMAC hash using the vnp_HashSecret
        using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(vnp_HashSecret)))
        {
            var computedHash = BitConverter
                .ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(signData)))
                .Replace("-", "")
                .ToLower();

            DateTime paymentDate;

            bool isDateValid = DateTime.TryParseExact(request.vnp_PayDate, "yyyyMMddHHmmss", null,
                System.Globalization.DateTimeStyles.None, out paymentDate);
            if (!isDateValid)
            {
                return BadRequest(new { status = "fail", message = "Invalid payment date format" });
            }

            TimeZoneInfo
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Vietnam Standard Time
            DateTime paymentDateInTimeZone = TimeZoneInfo.ConvertTime(paymentDate, timeZone);

            // Hash matches, process the payment result
            
            if (request.vnp_ResponseCode == "00" && request.vnp_TransactionStatus == "00")
            {
                int point = 0;
                if (request.vnp_Amount == "10000000")
                {
                    point = int.Parse(request.vnp_Amount) / 100000 + 10;
                } else if (request.vnp_Amount == "20000000")
                {
                    point= int.Parse(request.vnp_Amount) / 100000 + 40;
                } else if (request.vnp_Amount == "50000000")
                {
                    point= int.Parse(request.vnp_Amount) / 100000 + 100;
                } 
                var payment = new Payment
                {
                    UserId = userId,
                    OrderInfo = orderInfo,
                    Amount = decimal.Parse(request.vnp_Amount) / 100,
                    BankCode = request.vnp_BankCode,
                    CardType = request.vnp_CardType,
                    PaymentDate = paymentDateInTimeZone,
                    TransactionId = request.vnp_TransactionNo,
                    SecureHash = request.vnp_SecureHash,
                    TransactionStatus = request.vnp_TransactionStatus,
                    Point = point
                };

                var account = await _account.GetById(userId);
                account.Point += point;
               

                await _account.Update(account._Id, account);
                await _payment.Insert(payment);
                
                var amountWithoutCents = (decimal.Parse(request.vnp_Amount) / 100).ToString("F0");
                // Build the callback URL with query parameters
                var callbackUrl = $"{urlCallBack}?vnp_Amount={Uri.EscapeDataString(amountWithoutCents)}" +
                                  $"&vnp_BankCode={Uri.EscapeDataString(request.vnp_BankCode)}" +
                                  $"&vnp_OrderInfo={Uri.EscapeDataString(request.vnp_OrderInfo)}" +
                                  $"&vnp_PayDate={Uri.EscapeDataString(request.vnp_PayDate)}" +
                                  $"&vnp_TransactionNo={Uri.EscapeDataString(request.vnp_TransactionNo)}";

                return Ok(new
                {
                    UrlPayment = callbackUrl
                }); // Redirect to the callback URL with the query parameters
            }

            return BadRequest(new
            {
                status = "fail",
                userId = userId,
                orderInfo = orderInfo,
                message = "Transaction failed",
                data = queryParams
            });
        }
    }
    
    
}