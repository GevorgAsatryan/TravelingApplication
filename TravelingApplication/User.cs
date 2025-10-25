using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
