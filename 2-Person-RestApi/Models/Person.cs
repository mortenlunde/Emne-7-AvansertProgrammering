namespace Person_RestApi.Models;

public class Person
{
    public long Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public int Age { get; init; }
}