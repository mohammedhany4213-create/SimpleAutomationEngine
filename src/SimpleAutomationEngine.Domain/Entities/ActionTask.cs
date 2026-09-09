using System.ComponentModel.DataAnnotations ;
using SimpleAutomationEngine.Domain.Enums ;
using System.ComponentModel.DataAnnotations.Schema;


namespace SimpleAutomationEngine.Domain.Entities
{
    public class ActionTask
    {
        [Key]
        public int ActionTaskId {get; set;}

        public int UserId {get; set;}

        [Required]
        [MaxLength(500)]
        public string Content {get; set;} = string.Empty ;

        [Required]
        public ActionType Type {get; set;}

        public ActionStatus Status {get; set;}

        [Required]
        public DateTime ExecusionTime {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow ;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public ICollection<ActionLog> ActionLogs {get; set;} = new List<ActionLog>();

    }
}