using System.ComponentModel.DataAnnotations;

namespace Task1.Dtos;

public class CreateCustomerRequest
{
    [Required, StringLength(50)]
    public string Name { get; set; } = default!;

    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [StringLength(20)]
    public string? Phone { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool? IsActive { get; set; } 
}
