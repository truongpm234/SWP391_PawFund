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
<<<<<<< HEAD
            // Tìm kiếm UserRole hiện tại của người dùng
=======
>>>>>>> Dev-for-test
            var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
            if (userRole == null)
            {
                throw new Exception("User role not found.");
            }

<<<<<<< HEAD
            // Xóa quyền hiện tại của người dùng
            _dbContext.UserRoles.Remove(userRole);

            // Tìm người dùng
=======
            _dbContext.UserRoles.Remove(userRole);

>>>>>>> Dev-for-test
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

<<<<<<< HEAD
            // Cập nhật trạng thái người dùng
            user.IsApproved = false;
            user.IsApprovedUser = false;

            // Tìm role với RoleId = 2 (user)
=======
            user.IsApproved = false;
            user.IsApprovedUser = false;

>>>>>>> Dev-for-test
            var userRoleId = 2;
            var userRoleForUser = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == userRoleId);
            if (userRoleForUser == null)
            {
                throw new Exception("Role 'user' not found.");
            }

<<<<<<< HEAD
            // Gán lại RoleId = 2
=======
            // gan role id 2
>>>>>>> Dev-for-test
            var newUserRole = new UserRole { UserId = userId, RoleId = userRoleForUser.RoleId };
            _dbContext.UserRoles.Add(newUserRole);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

<<<<<<< HEAD
            return user; // Trả về user
=======
            return user;
>>>>>>> Dev-for-test
        }

        public async Task<List<User>> GetAllManagerRequests()
        {
<<<<<<< HEAD
            // Lấy danh sách tất cả user đã yêu cầu quyền manager nhưng chưa được phê duyệt
=======
            // lay list nhung user chua duoc duyet manager
>>>>>>> Dev-for-test
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
<<<<<<< HEAD
            // Kiểm tra xem người dùng đã yêu cầu và chưa được phê duyệt quyền manager
=======

>>>>>>> Dev-for-test
            if (!user.IsApprovedUser || user.IsApproved)
            {
                throw new Exception("This user has not requested or is already approved for the manager role.");
            }

<<<<<<< HEAD
            // Phê duyệt quyền "manager"
=======
            // duyet manager
>>>>>>> Dev-for-test
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "manager");
            if (role == null)
            {
                throw new Exception("Manager role not found.");
            }

<<<<<<< HEAD
            // Xóa quyền cũ (nếu có) của người dùng
=======
>>>>>>> Dev-for-test
            var existingUserRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == 2);
            if (existingUserRole != null)
            {
                _dbContext.UserRoles.Remove(existingUserRole);
            }

<<<<<<< HEAD
            // Thêm quyền mới (manager)
            var userRole = new UserRole { UserId = user.UserId, RoleId = role.RoleId };
            _dbContext.UserRoles.Add(userRole);

            // Đánh dấu user đã được phê duyệt
=======
            var userRole = new UserRole { UserId = user.UserId, RoleId = role.RoleId };
            _dbContext.UserRoles.Add(userRole);

>>>>>>> Dev-for-test
            user.IsApproved = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

<<<<<<< HEAD
            //email thong bao
=======
>>>>>>> Dev-for-test
            var userEmail = user.Email;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("Email not found.");
            }

<<<<<<< HEAD
            // gửi email thông báo
=======
>>>>>>> Dev-for-test
            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Request Manager role Notification from PawFund",
                Body = "Your request manager role status has been changed."
            };

<<<<<<< HEAD
            await _emailService.SendEmaiRequestRoleAsync(mailrequest);
=======
            await _emailService.SendEmail(mailrequest);
>>>>>>> Dev-for-test

            return user;
        }

<<<<<<< HEAD

    }

}
=======
        public async Task<bool> DeleteUser(int userId)
        {
            // Fetch the user along with related UserRoles
            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // remove all related UserRoles
            _dbContext.UserRoles.RemoveRange(user.UserRoles);

            // remove user
            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return true;
        }

    }

}
>>>>>>> Dev-for-test
