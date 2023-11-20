using Models.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Demo.Models.Entity
{
    public class QueryToSave
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Query { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser appUser { get; set; }
    }
}
