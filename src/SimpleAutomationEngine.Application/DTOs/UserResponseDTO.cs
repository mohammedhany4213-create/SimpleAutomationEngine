using System.ComponentModel.DataAnnotations;

namespace SimpleAutomationEngine.Application.DTos;

public class UserResponse
{
    public int UserId {get; set;}
    public string FullName {get; set;} = string.Empty ;
    public string Email {get; set;} = string.Empty ;

    public DateTime CreatedAt {get; set;}
}