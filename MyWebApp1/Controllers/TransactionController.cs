﻿using Microsoft.AspNetCore.Mvc;
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
    public IActionResult CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int transactionId = _transactionService.CreateTransaction(
            request.TransactionAmount,
            request.UserId,
            request.TransactionTypeId);

        var vnpayUrl = _transactionService.GenerateVnpayUrl(transactionId, request.TransactionAmount);
        return Ok(new { vnpayUrl });
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