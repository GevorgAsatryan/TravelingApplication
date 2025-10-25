namespace TravelingApplication
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}

