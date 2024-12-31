using System.ComponentModel.DataAnnotations;

namespace Conference.Core.Models
{
    public class User
    {
        public long Id { get; set; }

        [StringLength(20, MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(40, MinimumLength = 3)]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; }

        public ConnectionStatus Status { get; set; }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            Status = ConnectionStatus.Disconnected;
        }

        public User Clone()
            => new(Name, Email, Password)
            {
                Id = Id,
                Status = Status
            };
    }
}
