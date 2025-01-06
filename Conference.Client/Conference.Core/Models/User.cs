using System.ComponentModel.DataAnnotations;

namespace Conference.Core.Models
{
    public class User : ICloneable
    {
        public long Id { get; set; }

        [StringLength(20, MinimumLength = 4)]
        public required string Name { get; set; }

        [StringLength(40, MinimumLength = 3)]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        [StringLength(40, MinimumLength = 8)]
        public required string Password { get; set; }

        public ConnectionStatus Status { get; set; } = ConnectionStatus.Disconnected;

        public object Clone()
            => new User()
            {
                Id = Id,
                Name = Name,
                Email = Email,
                Password = Password,
                Status = Status
            };
    }
}
