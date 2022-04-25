using System.ComponentModel.DataAnnotations;

namespace ClarityEmailer.API.Models;

public class EmailMessageDTO
{
    [Required]
    [Display(Name = "Recipient Address")]
    [EmailAddress(ErrorMessage = "The email address is not valid")]
    public string ToAddress { get; set; }
}