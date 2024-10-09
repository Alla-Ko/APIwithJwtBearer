using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeWork4Products.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    [Display(Name = "Name")]
    [StringLength(50)]
    public string? Name { get; set; }
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1024)]
    public string? Description { get; set; }
}
