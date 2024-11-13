namespace MyWebApp1.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
