using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Models.MyWebApp1.Models;
using MyWebApp1.Models;
using MyWebApp1.DTO;
using SendGrid.Helpers.Mail;

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

        public List<UserApprove> GetPendingApproveRequests()
        {
            var pendingRequests = _dbContext.UserApproves
                                             .Where(ua => ua.IsApprovedUser == false)
                                             .ToList();

            return pendingRequests;
        }


        public async Task<bool> ApproveUser(int approveId, bool isApproved)
        {
            var userApprove = await _dbContext.UserApproves.FindAsync(approveId);

            if (userApprove != null)
            {
                userApprove.IsApprovedUser = isApproved;

                var user = await _dbContext.Users.FindAsync(userApprove.UserId);
                if (user != null)
                {
                    user.IsApprovedUser = isApproved;
                }

                _dbContext.Entry(userApprove).State = EntityState.Modified;
                if (user != null)
                {
                    _dbContext.Entry(user).State = EntityState.Modified;
                }

                await _dbContext.SaveChangesAsync();

                if (user == null || string.IsNullOrEmpty(user.Email))
                {
                    throw new Exception("User email not found.");
                }

                var approvalStatus = isApproved ? "approved" : "denied";
                var mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,
                    Subject = "Approval Status Notification from PawFund",
                    Body = $"Hello {user.Username}, your information has been approved successfully. Thank you! From PawFund Team."
                };

                await _emailService.SendEmail(mailrequest);

                return true;
            }

            return false;
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
            var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
            if (userRole == null)
            {
                throw new Exception("User role not found.");
            }

            _dbContext.UserRoles.Remove(userRole);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.IsApproved = false;
            //user.IsApprovedUser = false;

            var userRoleId = 2;
            var userRoleForUser = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == userRoleId);
            if (userRoleForUser == null)
            {
                throw new Exception("Role 'user' not found.");
            }

            // gan role id 2
            var newUserRole = new UserRole { UserId = userId, RoleId = userRoleForUser.RoleId };
            _dbContext.UserRoles.Add(newUserRole);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> GetAllManagerRequests()
        {
            // lay list nhung user chua duoc duyet manager
            return await _dbContext.Users
                //.Where(u => u.IsApprovedUser && !u.IsApproved)
                .Where(u => !u.IsApproved)
                .ToListAsync();
        }

        public async Task<User> ApproveManagerRequest(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            //if (!user.IsApprovedUser || user.IsApproved)
            if (user.IsApproved)
                {
                throw new Exception("This user has not requested or is already approved for the manager role.");
            }

            // duyet manager
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "manager");
            if (role == null)
            {
                throw new Exception("Manager role not found.");
            }

            var existingUserRole = await _dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == 2);
            if (existingUserRole != null)
            {
                _dbContext.UserRoles.Remove(existingUserRole);
            }

            var userRole = new UserRole { UserId = user.UserId, RoleId = role.RoleId };
            _dbContext.UserRoles.Add(userRole);

            user.IsApproved = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            var userEmail = user.Email;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("Email not found.");
            }

            Mailrequest mailrequest = new Mailrequest
            {
                ToEmail = userEmail,
                Subject = "Request Manager role Notification from PawFund",
                Body = "Your request manager role status has been changed."
            };

            await _emailService.SendEmail(mailrequest);

            return user;
        }

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
