using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController()
    {
        // Sử dụng chuỗi kết nối của bạn để khởi tạo TransactionService
        string connectionString = "Your SQL Server connection string here";
        _transactionService = new TransactionService(connectionString);
    }

    [HttpPost("create")]
    public IActionResult CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        int transactionId = _transactionService.CreateTransaction(request.TransactionAmount, request.IsMoneyDonation, request.IsResourceDonation, request.UserId, request.TransactionTypeId);

        var vnpayUrl = _transactionService.GenerateVnpayUrl(transactionId, request.TransactionAmount);
        return Ok(new { vnpayUrl });
    }

    [HttpGet("callback")]
    public IActionResult VnpayCallback([FromQuery] VNPayResponseModel model)
    {
        var isUpdated = _transactionService.HandleVnpayCallback(model.vnp_ResponseCode, int.Parse(model.vnp_TxnRef));

        if (isUpdated)
        {
            return Ok("Transaction updated successfully");
        }
        return BadRequest("Transaction failed or invalid");
    }
}
