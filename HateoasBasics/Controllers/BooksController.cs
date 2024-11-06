using HateoasBasics.Models;
using Microsoft.AspNetCore.Mvc;

namespace HateoasBasics.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly LinkGenerator _generator;

    private static readonly List<BookDto> Books =
    [
        new() { Id = 1, Title = "Professional C# 6", Author = "James Newton-King" },
        new() {Id = 2, Title = "Professional C# 7", Author = "James Newton-King" },
    ];
    
    public BooksController(LinkGenerator generator)
    {
        _generator = generator;
    }
    
    [HttpGet("GetBooks")]
    public ActionResult<string> GetBooksAsync()
    {
        return Ok(Books);
    }

    [HttpGet("GetBooks/{id:int}")]
    public ActionResult<BookDto> GetBookById(int id)
    {
        BookDto? book = Books.FirstOrDefault(b => b.Id == id);
        
        if (book == null) return BadRequest("Book not found");
        
        // HATEOAS
        book.Links?.Add(
            new Link()
            {
                Href = _generator.GetPathByAction(
                    HttpContext,
                    action: nameof(GetBookById),
                    values: new { id = book.Id }),
                Method = "GET",
                Rel = "self"
            });
        
        return Ok(book);
    }
}