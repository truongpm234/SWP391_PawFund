using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using MyWebApp1.Data;
using MyWebApp1.Models;
using Cursus_Api.Helper;
public class TransactionService
{
    private readonly MyDbContext _context;
    private readonly IConfiguration _configuration;

    public TransactionService(MyDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public int CreateTransaction(decimal amount, int userId, int transactionTypeId, int shelterId, string note)
    {
        var transaction = new Transaction
        {
            TransactionAmount = amount,
            UserId = userId,
            TransactionStatusId = 1,
            TransactionTypeId = transactionTypeId,
            ShelterId = shelterId, // Sử dụng giá trị ShelterId
            Note = note // Sử dụng giá trị Note
        };
        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        return transaction.TransactionId;
    }

    public bool UpdateTransactionStatus(int transactionId, int statusId)
    {
        var transaction = _context.Transactions.Find(transactionId);
        if (transaction != null)
        {
            transaction.TransactionStatusId = statusId;
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool HandleVnpayCallback(string responseCode, int transactionId)
    {
        if (responseCode == "00") 
        {
            return UpdateTransactionStatus(transactionId, 2); 
        }
        return false;
    }

    public string GenerateVnpayUrl(int transactionId, decimal amount)
    {
        var vnp_TmnCode = _configuration["VnPAY:TmnCode"];
        var vnp_HashSecret = _configuration["VnPAY:HashSecret"];
        var vnp_ReturnUrl = _configuration["VnPAY:ReturnUrl"];
        var vnp_Url = _configuration["VnPAY:Url"];

        if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret) || string.IsNullOrEmpty(vnp_ReturnUrl) || string.IsNullOrEmpty(vnp_Url))
        {
            throw new ArgumentNullException("One or more configuration values are null or empty.");
        }

        VnPayLibrary vnPay = new VnPayLibrary();
        vnPay.AddRequestData("vnp_Version", "2.1.0");
        vnPay.AddRequestData("vnp_Command", "pay");
        vnPay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        vnPay.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString());
        vnPay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        vnPay.AddRequestData("vnp_CurrCode", "VND");
        vnPay.AddRequestData("vnp_IpAddr", "127.0.0.1");
        vnPay.AddRequestData("vnp_Locale", "vn");
        vnPay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {transactionId}");
        vnPay.AddRequestData("vnp_OrderType", "billpayment");
        vnPay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
        vnPay.AddRequestData("vnp_TxnRef", transactionId.ToString());
        vnPay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));

        string paymentUrl = vnPay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        Console.WriteLine("Generated Payment URL: " + paymentUrl);
        return paymentUrl;
    }
}