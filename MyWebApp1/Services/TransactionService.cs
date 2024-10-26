﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using MyWebApp1.Data;
using MyWebApp1.Models;
using Cursus_Api.Helper; // Add reference to VnPayLibrary

public class TransactionService
{
    private readonly MyDbContext _context;
    private readonly IConfiguration _configuration;

    public TransactionService(MyDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // Tạo giao dịch mới
    public int CreateTransaction(decimal amount, bool isMoneyDonation, bool isResourceDonation, int userId, int transactionTypeId)
    {
        var transaction = new Transaction
        {
            TransactionAmount = amount,
            IsMoneyDonation = isMoneyDonation,
            IsResourceDonation = isResourceDonation,
            UserId = userId,
            TransactionStatusId = 1, // 'Pending' status ID
            TransactionTypeId = transactionTypeId
        };

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        return transaction.TransactionId;
    }

    // Cập nhật trạng thái giao dịch
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

    // Xử lý phản hồi từ VNPAY
    public bool HandleVnpayCallback(string responseCode, int transactionId)
    {
        if (responseCode == "00") // Giao dịch thành công
        {
            return UpdateTransactionStatus(transactionId, 2); // 2 là trạng thái 'Thành công'
        }
        return false;
    }

    // Tạo URL cho VNPAY với mã hóa bảo mật
    public string GenerateVnpayUrl(int transactionId, decimal amount)
    {
        // Lấy thông tin từ appsettings.json
        var vnp_TmnCode = _configuration["VnPAY:TmnCode"];
        var vnp_HashSecret = _configuration["VnPAY:HashSecret"];
        var vnp_ReturnUrl = _configuration["VnPAY:ReturnUrl"];
        var vnp_Url = _configuration["VnPAY:Url"];

        if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret) || string.IsNullOrEmpty(vnp_ReturnUrl) || string.IsNullOrEmpty(vnp_Url))
        {
            throw new ArgumentNullException("One or more configuration values are null or empty.");
        }

        // Instantiate VnPayLibrary
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

        // Generate the secure URL
        string paymentUrl = vnPay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        Console.WriteLine("Generated Payment URL: " + paymentUrl);
        return paymentUrl;
    }
}