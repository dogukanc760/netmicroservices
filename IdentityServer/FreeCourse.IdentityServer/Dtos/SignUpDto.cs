using System.ComponentModel.DataAnnotations;

namespace FreeCourse.IdentityServer.Dtos
{
    public class SignUpDto
    {
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string City { get; set; }
    }
}
