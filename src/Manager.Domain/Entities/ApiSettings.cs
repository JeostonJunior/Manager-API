namespace Manager.Domain.Entities
{
    public class ApiSettings
    {
        public JwtSettings JwtSettings { get; set; }
        
        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
