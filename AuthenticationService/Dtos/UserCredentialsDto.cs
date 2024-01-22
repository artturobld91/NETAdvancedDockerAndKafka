using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AuthenticationService.Dtos
{
    public class UserCredentialsDto
    {
        [Required]
        [EmailAddress]
        [NotNull]
        public string UserName { get; set; }

        [Required]
        [PasswordPropertyText]
        [NotNull]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
