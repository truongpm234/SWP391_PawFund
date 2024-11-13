using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Services;
using System.Threading.Tasks;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly AdminService adminService;
        private readonly ManagerService managerService;
        private readonly MyDbContext myDbContext;

        public EmailController(IEmailService service, AdminService adminService, MyDbContext myDbContext)
        {
            this.emailService = service;
            this.adminService = adminService;
            this.myDbContext = myDbContext;
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("SendEmail-AddNewPet-by-Manager")]
        public async Task<IActionResult> SendEmailAddNewPet(int userId)
        {
            try
            {
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,
                    Subject = "Your request add pet on PawFund",
                    Body = "Your request add pet on PawFund is accepted!"
                };

                await emailService.SendEmail(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("SendEmail-AdoptionRequest-by-Staff")]
        public async Task<IActionResult> SendEmailAdoption(int userId)
        {
            try
            {
                // Lấy thông tin người dùng dựa trên userId
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email, 
                    Subject = "Your request adoption pet on PawFund",
                    Body = "<h1>\"Your request adopt pet on PawFund is accepted!\"</h1>"
                };

                await emailService.SendEmail(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("SendEmail-ManagerRoleRequest-by-Admin")]
        public async Task<IActionResult> SendEmaiRequestRoleAsync(int userId)
        {
            try
            {
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,
                    Subject = "Your request manager role on PawFund",
                    Body = "Your request manager role on PawFund is accepted!"
                };

                await emailService.SendEmail(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("SendEmail-request-Event-by-Manager")]
        public async Task<IActionResult> SendEmaiAcceptEvent(int userId)
        {
            try
            {
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,
                    Subject = "Your request ogranize event on PawFund",
                    Body = "Your request ogranize event on PawFund is accepted!"
                };

                await emailService.SendEmail(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost("SendEmail-request-AddNewPet-by-User")]
        public async Task<IActionResult> SendEmailAddPetRequest(int userId)
        {
            try
            {
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,
                    Subject = "Your request to add a new pet on PawFund.",
                    Body = "Your request to add a pet to the website is waiting for acceptance. Please wait, and thank you for your contribution!"
                };

                await emailService.SendEmail(mailrequest);

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("SendEmail-delete-Pet-by-Manager")]
        public async Task<IActionResult> SendEmailDeletePetResponse(int petId)
        {
            try
            {
                var pet = await myDbContext.Pets
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.PetId == petId);

                if (pet == null || pet.User == null)
                {
                    return NotFound("Pet or user not found.");
                }

                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = pet.User.Email,
                    Subject = "Notification about your pet on PawFund",
                    Body = "Your Pet on PawFund has been deleted by a manager due to invalid information. For more details, contact us at PawFundPet@gmail.com."
                };

                await emailService.SendEmail(mailrequest);
                return Ok("Notification email sent to the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
