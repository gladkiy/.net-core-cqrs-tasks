using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.DBModels.Entities
{
    [Table("Tasks")]
    public class Task
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public bool Completed { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }
    }
}