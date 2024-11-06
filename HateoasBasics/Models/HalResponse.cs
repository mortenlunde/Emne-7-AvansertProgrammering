namespace HateoasBasics.Models;

public abstract class HalResponse
{
    public List<Link>? Links { get; set; } = [];
}