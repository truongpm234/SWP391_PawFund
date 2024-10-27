using MyWebApp1.DTO;

namespace MyWebApp1.Services
{
    public interface IEmailService
    {
        Task SendEmailAddNewPetAsync(Mailrequest mailrequest);    
        Task SendEmailAdoptionAsync(Mailrequest mailrequest);    
        Task SendEmaiRequestRoleAsync(Mailrequest mailrequest);
        Task SendEmaiAcceptEvent(Mailrequest mailrequest);
        Task SendEmaiAddPetRequest(Mailrequest mailrequest);

    }
}
