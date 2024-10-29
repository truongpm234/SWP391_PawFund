using MyWebApp1.DTO;

namespace MyWebApp1.Services
{
    public interface IEmailService
    {
        Task SendEmail(Mailrequest mailrequest);

    }
}
