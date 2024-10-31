namespace MyWebApp1.DTO

{
    public class VNPayResponseModel
    {
        public string vnp_ResponseCode { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_Amount { get; set; }

        public string Token { get; set; }

        public string TransactionId { get; set; }
    }

    public class VnPaymentRequestModel
    {
        public bool FullName { set; get; }
        public string Description { set; get; }

        public string Amount { set; get; }
    }
}