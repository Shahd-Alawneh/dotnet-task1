namespace Task1.Models;

public class Customer
{
    public int Id { get; set; }                     // Primary key (Auto)
    public string Name { get; set; } = default!;    // Required, max length 50
    public string Email { get; set; } = default!;   // Required, valid email format
    public string? Phone { get; set; }              // Optional, max length 20
    public DateTime? DateOfBirth { get; set; }      // Optional
    public bool IsActive { get; set; } = true;      // Default true
    public DateTime CreatedAt { get; set; }         // Auto-set on creation
}
