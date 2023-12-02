using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    public class History
    {
        public Guid Id { get; set; }
        public string Query { get; set; }
        public DateTime ExecutedTime { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser appUser { get; set; }
    }
}
