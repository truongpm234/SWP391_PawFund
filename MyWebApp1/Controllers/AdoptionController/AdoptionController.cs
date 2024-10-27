using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.DTO;
using MyWebApp1.Models;
using MyWebApp1.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyWebApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionController : ControllerBase
    {
        private readonly AdoptionService _adoptionService;
        private readonly MyDbContext _context;

        public AdoptionController(AdoptionService adoptionService, MyDbContext context)
        {
            _adoptionService = adoptionService;
            _context = context;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost("adoption-request/{petId}")]
        public IActionResult CreateAdoptionRequest(int petId, [FromBody] AdoptionRequestModel request)
        {
            try
            {
                // Lấy thông tin user từ JWT token trong HttpContext
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim not found in token.");
                }

                // Parse userId từ claim
                var userId = int.Parse(userIdClaim.Value);

                // Gán PetId từ URL vào request
                // request.PetId = petId; // Bỏ dòng này vì đã loại bỏ PetId trong AdoptionRequestModel

                // Xử lý yêu cầu với userId
                bool success = _adoptionService.CreateAdoptionRequest(request, userId, petId); // Cập nhật phương thức gọi dịch vụ để truyền petId

                return success ? Ok("Adoption request created successfully.") : StatusCode(500, "Failed to create adoption request.");
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi để kiểm tra
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An internal error occurred.");
            }
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("user-adoption-requests")]
        public IActionResult GetMyAdoptionRequests()
        {
            try
            {
                // Lấy thông tin user từ JWT token trong HttpContext
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim not found in token.");
                }

                // Parse userId từ claim
                var userId = int.Parse(userIdClaim.Value);

                // Lấy danh sách yêu cầu nhận nuôi của người dùng từ service
                var userAdoptions = _adoptionService.GetAdoptionsByUser(userId);

                return Ok(userAdoptions);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi để kiểm tra
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An internal error occurred.");
            }
        }

    }
}

//[Authorize(Policy = "ManagerOnly")]
//[HttpGet("get-all-adoptions")]
//public IActionResult GetAdoptions()
//{
//    var adoptions = _adoptionService.GetAdoptions();
//    return Ok(adoptions);
//}

//[Authorize(Policy = "ManagerOnly")]
//[HttpPut("approve/{adoptionId}")]
//public IActionResult ApproveAdoption(int adoptionId)
//{
//    bool success = _adoptionService.ApproveAdoption(adoptionId);
//    return success ? Ok("Adoption approved.") : NotFound("Adoption not found.");
//}


