namespace Person_RestApi.Models;

public class Person
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int age { get; set; }
}