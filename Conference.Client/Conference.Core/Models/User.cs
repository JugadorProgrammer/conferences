using System.ComponentModel.DataAnnotations;

namespace Conference.Core.Models
{
    public class User
    {
        public long Id { get; set; }

        [StringLength(20, MinimumLength = 4)]
        public required string Name { get; set; }

        [StringLength(40, MinimumLength = 3)]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        [StringLength(40, MinimumLength = 8)]
        public required string Password { get; set; }

        public required ConnectionStatus Status { get; set; }

        public User Clone()
            => new()
            {
                Id = Id,
                Name = Name,
                Email = Email,
                Password = Password,
                Status = Status
            };
    }
}
