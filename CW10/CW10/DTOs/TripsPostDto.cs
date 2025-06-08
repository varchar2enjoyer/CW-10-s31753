using System.ComponentModel.DataAnnotations;
namespace CW10.DTOs;

public class TripsPostDto
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Telephone { get; set; } = null!;
    [Required]
    public string Pesel { get; set; } = null!;
    [Required]
    public int IdTrip { get; set; }
    [Required]
    public string TripName { get; set; } = null!;
    [Required]
    public DateTime PaymentDate { get; set; }
}