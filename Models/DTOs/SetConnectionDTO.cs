
namespace Models.DTOs
{
    public class SetConnectionDTO
    {
        public string serverName { get; set; }
        public string databaseName { get; set; }

        public string? username { get; set; }
        public string? password { get; set; }

        public bool requiresCredentials { get; set; }
    }
}
