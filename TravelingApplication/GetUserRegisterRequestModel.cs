using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetUserRegisterRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
