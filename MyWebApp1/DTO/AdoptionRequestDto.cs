namespace MyWebApp1.DTO
{
    public class AdoptionRequestDto
    {
        public int AdoptionId { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public bool IsApproved { get; set; }
        public string? Note { get; set; }
        public string? Username { get; set; }
        public string? PetName { get; set; }
    }

    public class PetDto
    {
        public int PetId { get; set; }
        public string? PetName { get; set; }
        public bool IsApproved { get; set; }
    }
}
