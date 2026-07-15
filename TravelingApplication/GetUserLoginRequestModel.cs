using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetUserLoginRequestModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
