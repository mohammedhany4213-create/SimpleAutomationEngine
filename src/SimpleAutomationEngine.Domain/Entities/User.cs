using System.ComponentModel.DataAnnotations ;

namespace SimpleAutomationEngine.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required]
        [MaxLength(100)]
        public string FullName {get; set;} = string.Empty ;

        [Required]
        [EmailAddress]
        public string Email {get; set;} = string.Empty ;

        [Required]
        public string PasswordHash {get; set;}  = string.Empty ;

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow ;

        public bool IsActive {get; set;} = true ;

        public string? RefreshToken {get; set;}  
        public DateTime? RefreshTokenExpirationTime {get; set;}  

        public ICollection<ActionTask> ActionTasks {get; set;} = new List<ActionTask>();
    }
} 