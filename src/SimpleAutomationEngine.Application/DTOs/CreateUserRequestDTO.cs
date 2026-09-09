using System.ComponentModel.DataAnnotations;
namespace SimpleAutomationEngine.Application.DTos;

public class CreateUser
{
    [Required (ErrorMessage = "Name is Required .")]
    [MaxLength(100 , ErrorMessage = "Name cant exceed 100 character")]
    public string FullName {get; set;} = string.Empty ;

    [Required (ErrorMessage = "Email is Required .")]
    [EmailAddress (ErrorMessage = "Invalid email address format .")]
    public string Email {get; set;} = string.Empty ;
}