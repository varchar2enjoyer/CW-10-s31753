namespace CW10.DTOs;

public class SingleTripGetDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public IEnumerable<CountryGetDto> Countries { get; set; } = null!;
    public IEnumerable<ClientGetDto> Clients { get; set; } = null!;
}