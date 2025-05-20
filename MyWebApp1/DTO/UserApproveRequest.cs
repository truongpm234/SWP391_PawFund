namespace MyWebApp1.DTO
{
    public class UserApproveRequest
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Occupation { get; set; }
        public string IDCardNumber { get; set; }
        public string PetCareCapacity { get; set; }
        public DateTime? DateGet { get; set; }
        public string PlaceGet { get; set; }
        public string UsualAddress { get; set; }
    }

    //public class UserApproveRequest
    //{
    //    public int UserId { get; set; }
    //    public string Address { get; set; }
    //    public string PhoneNumber { get; set; }
    //    public string Occupation { get; set; }
    //    public string IDCardNumber { get; set; }
    //    public string PetCareCapacity { get; set; }
    //    public DateTime? DateGet { get; set; }
    //    public string PlaceGet { get; set; }
    //    public string UsualAddress { get; set; }
    //    public bool IsApprovedUser { get; set; }
    //}

}
