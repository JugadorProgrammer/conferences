namespace Conference.Core.Models
{
    public class User
    {
        // TODO: add regular expressions to string fields
        public required UserId Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required ConnectionStatus Status { get; set; }

        public User Clone()
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
