using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    public class PersonalConnection
    {
        public Guid Id { get; set; }
        public string serverName { get; set; }
        public string databaseName { get; set; }

        public string? username { get; set; }
        public string? password { get; set; }

        public string belongsTo { get; set; }
        [ForeignKey("belongsTo")]
        public AppUser appUser { get; set; }
    }
}
