using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.DBModels.Entities
{
    [Table("ErrorLogs")]
    public class ErrorLog
    {
        public int Id { get; set; }

        public DateTime LoggedDate { get; set; }

        public int? UserId { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string ErrorSummary { get; set; }

        [Required]
        public string ErrorDetail { get; set; }

        [Required]
        [MaxLength(1024)]
        public string ErrorLocation { get; set; }

        public User User { get; set; }
    }
}