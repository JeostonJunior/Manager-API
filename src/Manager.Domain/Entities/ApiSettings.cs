namespace Manager.Domain.Entities
{
    public class ApiSettings
    {
        public JwtSettings JwtSettings { get; set; }
        
        public ConnectionString ConnectionString { get; set; }
    }
}
