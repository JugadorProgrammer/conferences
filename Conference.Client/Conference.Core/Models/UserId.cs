namespace Conference.Core.Models
{
    public class UserId
    {
        public const char Separator = '-';

        public required long Id { get; set; }

        public required string ServerId { get; set; }

        public override string ToString()
            => $"{Id}-{ServerId}";

        public static bool TryParce(string data, out UserId? userId)
        {
            var split = data.Split(Separator);
            if (split.Length != 2 || !long.TryParse(split[0], out var Id))
            {
                userId = null;
                return false;
            }

            userId = new UserId() 
            {
                Id = Id,
                ServerId = split[1]
            };
            return true;
        }
    }
}
