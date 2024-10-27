using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using System.Security.Claims;

namespace MyWebApp1.Services
{
    public class DonationEventService
    {
        private readonly MyDbContext _dbContext;
        private readonly IEmailService _emailService;

        public DonationEventService(MyDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<bool> CreateDonationEventAsync(DonationEvent newEvent)
        {
            // Thêm sự kiện mới vào DB
            _dbContext.DonationEvents.Add(newEvent);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<DonationListDto>> GetAllEventsAsync()
        {
            return await _dbContext.DonationEvents
                .Select(eventItem => new DonationListDto
                {
                    EventId = eventItem.EventId,
                    EventName = eventItem.EventName,
                    EventContent = eventItem.EventContent,
                    EventStartDate = eventItem.EventStartDate,
                    EventEndDate = eventItem.EventEndDate,
                    IsEnded = eventItem.IsEnded,
                    IsApproved = eventItem.IsApproved
                })
                .ToListAsync();
        }


        public async Task<bool> ApproveEventAsync(int eventId)
        {
            var eventItem = await _dbContext.DonationEvents.FindAsync(eventId);

            if (eventItem == null || eventItem.IsApproved)
            {
                return false;
            }

            eventItem.IsApproved = true;
            await _dbContext.SaveChangesAsync();

            // lấy thông tin từ UserCreatedId
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == eventItem.UserCreatedId);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new Exception("Email not found.");
            }

            // gửi email thông báo
            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = user.Email,
                Subject = "Request Create Event Notification from PawFund",
                Body = "Your Event has been approved by admin!"
            };

            await _emailService.SendEmaiAcceptEvent(mailrequest);
            return true;
        }


        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var eventItem = await _dbContext.DonationEvents.FindAsync(eventId);

            if (eventItem == null)
            {
                return false;
            }

            _dbContext.DonationEvents.Remove(eventItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }


}
