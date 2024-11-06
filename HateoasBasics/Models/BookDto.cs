namespace HateoasBasics.Models;

public class BookDto : HalResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
}