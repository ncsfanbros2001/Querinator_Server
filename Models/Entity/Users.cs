using System.ComponentModel.DataAnnotations;

namespace Models.Entity
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
