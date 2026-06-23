using Microsoft.EntityFrameworkCore;

namespace TravelingApplication
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public async Task<User> FindUser(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUserToken(User user, string token)
        {
            if (user != null)
            {
                user.Token = token;
                await _context.SaveChangesAsync();
            }
        }
    }
}

