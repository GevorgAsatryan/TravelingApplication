using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class User : IdentityUser
    {
        public string? Token { get; set; }
    }
}
