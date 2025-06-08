namespace CW10.DTOs;

public class TripsGetDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public IEnumerable<SingleTripGetDto> Trips { get; set; } = null!;
}