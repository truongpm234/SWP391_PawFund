using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.Models;
using MyWebApp1.DTO;

namespace MyWebApp1.Services
{
    public class AdminService
    {
        private readonly MyDbContext _dbContext;
        private readonly IEmailService _emailService;

        public AdminService(MyDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public List<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _dbContext.Users.FirstOrDefault(x => x.UserId == id);
        }

        public async Task<User> RemoveUserRole(int userId)
        {
            // Tìm kiếm UserRole hiện tại của người dùng
            var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
            if (userRole == null)
            {
                throw new Exception("User role not found.");
            }

            // Xóa quyền hiện tại của người dùng
            _dbContext.UserRoles.Remove(userRole);

            // Tìm người dùng
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Cập nhật trạng thái người dùng
            user.IsApproved = false;
            user.IsApprovedUser = false;

            // Tìm role với RoleId = 2 (user)
            var userRoleId = 2;
            var userRoleForUser = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == userRoleId);
            if (userRoleForUser == null)
            {
                throw new Exception("Role 'user' not found.");
            }

            // Gán lại RoleId = 2
            var newUserRole = new UserRole { UserId = userId, RoleId = userRoleForUser.RoleId };
            _dbContext.UserRoles.Add(newUserRole);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user; // Trả về user
        }

        public async Task<List<User>> GetAllManagerRequests()
        {
            // Lấy danh sách tất cả user đã yêu cầu quyền manager nhưng chưa được phê duyệt
            return await _dbContext.Users
                .Where(u => u.IsApprovedUser && !u.IsApproved)
                .ToListAsync();
        }

        public async Task<User> ApproveManagerRequest(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            // Kiểm tra xem người dùng đã yêu cầu và chưa được phê duyệt quyền manager
            if (!user.IsApprovedUser || user.IsApproved)
            {
                throw new Exception("This user has not requested or is already approved for the manager role.");
            }

            // Phê duyệt quyền "manager"
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "manager");
            if (role == null)
            {
                throw new Exception("Manager role not found.");
            }

            // Xóa quyền cũ (nếu có) của người dùng
            var existingUserRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == 2);
            if (existingUserRole != null)
            {
                _dbContext.UserRoles.Remove(existingUserRole);
            }

            // Thêm quyền mới (manager)
            var userRole = new UserRole { UserId = user.UserId, RoleId = role.RoleId };
            _dbContext.UserRoles.Add(userRole);

            // Đánh dấu user đã được phê duyệt
            user.IsApproved = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            //email thong bao
            var userEmail = user.Email;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("Email not found.");
            }

            // gửi email thông báo
            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Request Manager role Notification from PawFund",
                Body = "Your request manager role status has been changed."
            };

            await _emailService.SendEmaiRequestRoleAsync(mailrequest);

            return user;
        }


    }

}