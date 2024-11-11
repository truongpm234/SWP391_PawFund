using MyWebApp1.DTO;

namespace MyWebApp1.Services
{
    public interface IEmailService
    {
<<<<<<< HEAD
        Task SendEmailAddNewPetAsync(Mailrequest mailrequest);
        Task SendEmailAdoptionAsync(Mailrequest mailrequest);
        Task SendEmaiRequestRoleAsync(Mailrequest mailrequest);
        Task SendEmaiAcceptEvent(Mailrequest mailrequest);
        Task SendEmaiAddPetRequest(Mailrequest mailrequest);

    }
}
=======
        Task SendEmail(Mailrequest mailrequest);

    }
}
>>>>>>> Dev-for-test
