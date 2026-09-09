using System.ComponentModel.DataAnnotations ;
using System.ComponentModel.DataAnnotations.Schema;
using SimpleAutomationEngine.Domain.Enums;

namespace SimpleAutomationEngine.Domain.Entities
{
    public class ActionLog
    {
        [Key]
        public int ActionLogId {get; set;}
        public int ActionTaskId {get; set;}

        public ActionStatus OldStatus {get; set;}

        public ActionStatus NewStatus {get; set;}

        public string? Notes { get; set; }

        public DateTime Timestamp {get; set;} = DateTime.UtcNow ;

        [ForeignKey(nameof(ActionTaskId))]
        public ActionTask ActionTask {get; set;}= null! ;
    }
}