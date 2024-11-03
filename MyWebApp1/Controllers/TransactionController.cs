using Microsoft.AspNetCore.Mvc;
using MyWebApp1.Services;
using MyWebApp1.Models;
using Microsoft.Extensions.Configuration;
using Cursus_Api.Helper;
using MyWebApp1.DTO;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;
    private readonly IConfiguration _configuration;

    public TransactionController(TransactionService transactionService, IConfiguration configuration)
    {
        _transactionService = transactionService;
        _configuration = configuration;
    }

    [HttpPost("create")]
    [Authorize] // Đảm bảo phương thức yêu cầu xác thực
    public IActionResult CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        try
        {
            // Lấy thông tin user ID từ JWT
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found in token.");
            }

            // Kiểm tra và parse userId từ claim
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid User ID.");
            }

            // Tạo giao dịch
            int transactionId = _transactionService.CreateTransaction(
                request.TransactionAmount,
                userId, // Sử dụng userId lấy từ JWT
                request.TransactionTypeId,
                request.ShelterId, // Thêm giá trị ShelterId
                request.Note // Thêm giá trị Note
            );

            // Tạo URL thanh toán
            var vnpayUrl = _transactionService.GenerateVnpayUrl(transactionId, request.TransactionAmount);
            return Ok(new { vnpayUrl });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(500, "An internal error occurred.");
        }
<<<<<<< HEAD

        int transactionId = _transactionService.CreateTransaction(
            request.TransactionAmount,
            request.UserId,
            request.TransactionTypeId);

        var vnpayUrl = _transactionService.GenerateVnpayUrl(transactionId, request.TransactionAmount);
        return Ok(new { vnpayUrl });
=======
>>>>>>> origin/Dat1
    }

    [HttpGet("callback")]
    public IActionResult VnpayCallback([FromQuery] VNPayResponseModel model)
    {
        var vnpay = new VnPayLibrary();
        foreach (var key in Request.Query.Keys)
        {
            vnpay.AddResponseData(key, Request.Query[key]);
        }

        string vnp_HashSecret = _configuration["VnPAY:HashSecret"];
        string secureHash = Request.Query["vnp_SecureHash"];

        // Validate signature
        bool isValidSignature = vnpay.ValidateSignature(secureHash, vnp_HashSecret);
        if (!isValidSignature)
        {
            return BadRequest("Invalid signature");
        }

        string responseCode = vnpay.GetResponseData("vnp_ResponseCode");
        int transactionId = int.Parse(vnpay.GetResponseData("vnp_TxnRef"));

        if (responseCode == "00")
        {
            _transactionService.UpdateTransactionStatus(transactionId, 2);
            return Ok("Transaction updated successfully");
        }
        else
        {
            return BadRequest("Transaction failed or invalid");
        }
    }
}