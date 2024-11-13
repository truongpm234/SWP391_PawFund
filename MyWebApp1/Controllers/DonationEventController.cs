using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Payload;
using MyWebApp1.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyWebApp1.Controllers
{
    [ApiController]
    [Route("api/donation-events")]
    public class DonationEventController : ControllerBase
    {
        private readonly DonationEventService _donationEventService;

        public DonationEventController(DonationEventService donationEventService)
        {
            _donationEventService = donationEventService;
        }

        [Authorize(Policy = "StaffOnly")]
        [HttpPost("create-event-request-by-staff")]
        public async Task<IActionResult> CreateEvent([FromBody] DonationEventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Lấy thông tin user từ JWT token
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Tạo DonationEvent mới với UserCreatedId được lấy từ userId
            var newEvent = new DonationEvent
            {
                EventName = eventDto.EventName,
                EventContent = eventDto.EventContent,
                EventStartDate = eventDto.EventStartDate,
                EventEndDate = eventDto.EventEndDate,
                UserCreatedId = userId,
                IsApproved = false,
                IsEnded = false
            };

            var result = await _donationEventService.CreateDonationEventAsync(newEvent);

            if (result)
            {
                return Ok(new { message = "Event created successfully, awaiting approval." });
            }
            else
            {
                return BadRequest(new { message = "Failed to create event." });
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpGet("Get-list-event-by-manager")]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _donationEventService.GetAllEventsAsync();
            return Ok(events);
        }


        [Authorize(Policy = "ManagerOnly")]
        [HttpPut("approve-event-by-manager/{eventId}")]
        public async Task<IActionResult> ApproveEvent(int eventId)
        {
            var result = await _donationEventService.ApproveEventAsync(eventId);

            if (result)
            {
                return Ok(new { message = "Event approved successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to approve event." });
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpDelete("delete-event-by-manager/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var result = await _donationEventService.DeleteEventAsync(eventId);

            if (result)
            {
                return Ok(new { message = "Event deleted successfully." });
            }
            else
            {
                return BadRequest(new { message = "Failed to delete event." });
            }
        }

    }

}
