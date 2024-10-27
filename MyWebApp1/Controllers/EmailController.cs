using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
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


        public EmailController(IEmailService service, AdminService adminService)
        {
            this.emailService = service;
            this.adminService = adminService;
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("SendEmail-AddNewPet-by-Manager")]
        public async Task<IActionResult> SendEmailAddNewPet(int userId)  // Thêm userId để lấy email của người dùng
        {
            try
            {
                // Lấy thông tin người dùng dựa trên userId
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Tạo mail request với email người dùng lấy từ database
                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,  // Lấy email từ thông tin người dùng
                    Subject = "Your request add pet on PawFund",
                    Body = "Your request add pet on PawFund is accepted!"
                };

                await emailService.SendEmailAddNewPetAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần) và trả về thông báo lỗi
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("SendEmail-AdoptionRequest-by-Staff")]
        public async Task<IActionResult> SendEmailAdoption(int userId)  // Thêm userId để lấy email của người dùng
        {
            try
            {
                // Lấy thông tin người dùng dựa trên userId
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Tạo mail request với email người dùng lấy từ database
                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,  // Lấy email từ thông tin người dùng
                    Subject = "Your request adoption pet on PawFund",
                    Body = "<h1>\"Your request adopt pet on PawFund is accepted!\"</h1>"
                };

                await emailService.SendEmailAdoptionAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần) và trả về thông báo lỗi
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("SendEmail-ManagerRoleRequest-by-Admin")]
        public async Task<IActionResult> SendEmaiRequestRoleAsync(int userId)  // Thêm userId để lấy email của người dùng
        {
            try
            {
                // Lấy thông tin người dùng dựa trên userId
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Tạo mail request với email người dùng lấy từ database
                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,  // Lấy email từ thông tin người dùng
                    Subject = "Your request manager role on PawFund",
                    Body = "Your request manager role on PawFund is accepted!"
                };

                await emailService.SendEmaiRequestRoleAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần) và trả về thông báo lỗi
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "ManagerOnly")]
        [HttpPost("SendEmail-request-Event-by-Manager")]
        public async Task<IActionResult> SendEmaiAcceptEvent(int userId)
        {
            try
            {
                // Lấy thông tin người dùng dựa trên userId
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Tạo mail request với email người dùng lấy từ database
                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,  // Lấy email từ thông tin người dùng
                    Subject = "Your request ogranize event on PawFund",
                    Body = "Your request ogranize event on PawFund is accepted!"
                };

                await emailService.SendEmaiAcceptEvent(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần) và trả về thông báo lỗi
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost("SendEmail-request-AddNewPet-by-User")]
        public async Task<IActionResult> SendEmailAddPetRequest(int userId)
        {
            try
            {
                // Lấy thông tin người dùng dựa trên userId
                var user = adminService.GetUser(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Tạo mail request với email người dùng lấy từ database
                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,  // Lấy email từ thông tin người dùng
                    Subject = "Your request to add a new pet on PawFund.",
                    Body = "Your request to add a pet to the website is waiting for acceptance. Please wait, and thank you for your contribution!"
                };

                await emailService.SendEmaiAddPetRequest(mailrequest);

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
