namespace MyWebApp1.Payload.Response;

public class PetResponse
{
    public int PetId { get; set; }
    public string PetName { get; set; }
    public string PetType { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string MedicalCondition { get; set; }
    public string ContactPhoneNumber { get; set; }
    public string ContactEmail { get; set; }
    public string PetCategory { get; set; }
    public bool IsAdopted { get; set; }
    public bool IsApproved { get; set; }
}