namespace HomeWork4Products.Models;
using System.ComponentModel.DataAnnotations;


public class AuthModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
