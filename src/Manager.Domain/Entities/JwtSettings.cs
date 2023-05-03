namespace Manager.Domain.Entities
{
    public class JwtSettings
    {
        public string Key { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string HoursToExpire { get; set; }
    }
}
