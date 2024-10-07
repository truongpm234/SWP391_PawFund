namespace MyWebApp1.Payload;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}