using System.ComponentModel.DataAnnotations;

namespace Step3_WebApiConsumerConsole_Jwt.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? apiKey { get; set; }
    }

    //used for Login
    public class UserCredentials
    {
        [Required]
        public string? UserName { get; set; }  //Name or email

        [Required]
        public string? Password { get; set; }
    }

    //After successful Jwt Login
    public class JwtUserToken
    {
        public Guid TokenId { get; set; }

        public string? EncryptedToken { get; set; }
        public string? EncryptedRefreshToken { get; set; }  //Not used in this example
        public TimeSpan Validity { get; set; }
        public DateTime ExpiredTime { get; set; }

        //This will be the User part of the Claim
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
}
